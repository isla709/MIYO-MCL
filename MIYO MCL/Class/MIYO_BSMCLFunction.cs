using MinecraftLaunch.Classes.Interfaces;
using MinecraftLaunch.Classes.Models.Auth;
using MinecraftLaunch.Classes.Models.Launch;
using MinecraftLaunch.Components.Checker;
using MinecraftLaunch.Components.Launcher;
using MinecraftLaunch.Components.Resolver;
using MinecraftLaunch.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MIYO_MCL.Class
{
    public static class MIYO_BSMCLFunction
    {
        public async static Task<AccountData> GetUserData(MainWindow mainWindow)
        {
            

            var configdata = mainWindow.mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindow.mainWindowInit.AppconfigManager.ReadConfigFile());

            bool accountSafeMode = configdata.AccountSaveMode == "1" ? false : true;

            AccountData userData = await mainWindow.mainWindowInit.BSMCLUserManager.DeSerializationUser(accountSafeMode);
            return userData;

        }

        public async static void LoadUserList(MainWindow mainWindow)
        {
            try
            {
                var configdata = mainWindow.mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindow.mainWindowInit.AppconfigManager.ReadConfigFile());

                var userdata = await MIYO_BSMCLFunction.GetUserData(mainWindow);
                mainWindow.AccountList = userdata;

                if (mainWindow.AccountList.MicrosoftAccounts != null)
                {
                    mainWindow.AccountList.MicrosoftAccounts.ToList().ForEach(account =>
                    {
                        if (account.Uuid.ToString() == configdata.LastSelectionUuid) { mainWindow.selectedAccount = account; }
                    });
                }
                if (mainWindow.AccountList.OfflineAccounts != null)
                {
                    mainWindow.AccountList.OfflineAccounts.ToList().ForEach(account =>
                    {
                        if (account.Uuid.ToString() == configdata.LastSelectionUuid) { mainWindow.selectedAccount = account; }
                    });
                }
                if (mainWindow.AccountList.YggdrasilAccounts != null)
                {
                    mainWindow.AccountList.YggdrasilAccounts.ToList().ForEach(account =>
                    {
                        if (account.Uuid.ToString() == configdata.LastSelectionUuid) { mainWindow.selectedAccount = account; }
                    });
                }

                if (mainWindow.selectedAccount != null) { mainWindow.tb_selectuserName.Text = ((Account)mainWindow.selectedAccount).Name; }

                mainWindow.lb_UserList.Items.Clear();
                if (mainWindow.AccountList.MicrosoftAccounts != null)
                {
                    mainWindow.AccountList.MicrosoftAccounts.ToList().ForEach(account => mainWindow.lb_UserList.Items.Add(account));
                }
                if (mainWindow.AccountList.OfflineAccounts != null)
                {
                    mainWindow.AccountList.OfflineAccounts.ToList().ForEach(account => mainWindow.lb_UserList.Items.Add(account));
                }
                if (mainWindow.AccountList.YggdrasilAccounts != null)
                {
                    mainWindow.AccountList.YggdrasilAccounts.ToList().ForEach(account => mainWindow.lb_UserList.Items.Add(account));
                }

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        public async static void StartGame(MainWindow mainWindow)
        {
            if (mainWindow.selectedGameEntry == null)
            {
                mainWindow.Toast("请选择要启动的版本");
                return;
            }
            if (mainWindow.selectedAccount == null)
            {
                mainWindow.Toast("请选择要使用的账号");
                return;
            }


            mainWindow.btn_StartGame.Background = new SolidColorBrush(Colors.Red);
            mainWindow.btn_StartGame.IsEnabled = false;

            var configdata = mainWindow.mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindow.mainWindowInit.AppconfigManager.ReadConfigFile());

            LaunchConfig launchConfig = new LaunchConfig()
            {
                IsEnableIndependencyCore = true,
                JvmConfig = new JvmConfig(configdata.JavaPath) 
                {
                    MaxMemory = int.Parse(configdata.JvmMaxMemory),
                    MinMemory = int.Parse(configdata.JvmMinMemory)
                },
                LauncherName = "MIYO MCL",

            };

            switch (((Account)mainWindow.selectedAccount).Type)
            {
                case MinecraftLaunch.Classes.Enums.AccountType.Microsoft:

                    MicrosoftAccount StartGameAccount = await MIYO_BSMCL.ReflushMicrosoftUser((MicrosoftAccount)mainWindow.selectedAccount);

                    if (mainWindow.AccountList.MicrosoftAccounts != null)
                    {
                        mainWindow.AccountList.MicrosoftAccounts.RemoveAll(account => account.AccessToken == ((Account)mainWindow.selectedAccount).AccessToken);
                    }

                    if (mainWindow.AccountList.MicrosoftAccounts != null)
                    {
                        mainWindow.AccountList.MicrosoftAccounts.Add(StartGameAccount);
                    }

                    launchConfig.Account = StartGameAccount;
                    mainWindow.selectedAccount = StartGameAccount;

                    mainWindow.mainWindowInit.BSMCLUserManager.SerializationUserToFile(mainWindow.AccountList, mainWindow.mainWindowInit.BSMCLUserManager.SafeModeStatus);
                    await Task.Delay(500);
                    break;

                case MinecraftLaunch.Classes.Enums.AccountType.Offline:
                    launchConfig.Account = (OfflineAccount)mainWindow.selectedAccount;
                    break;
            }

            IGameResolver resolver;

            if (configdata.GamePath.Equals("SoftwareDirectory"))
            {
                resolver = new GameResolver(".minecraft");
            }
            else
            {
                resolver = new GameResolver(configdata.GamePath);
            }

            ResourceChecker resourceChecker = new ResourceChecker(mainWindow.selectedGameEntry);
            bool resourceResult = await resourceChecker.CheckAsync();
            if (!resourceResult)
            {
                mainWindow.Toast($"资源有缺失:{resourceChecker.MissingResources.Count}");

                List<IDownloadEntry> Missingcontent = resourceChecker.MissingResources.ToList();
                Missingcontent.ForEach(item =>
                {
                    Trace.WriteLine($"缺失资源： {item.Path} ", "ERROR");
                });
                Trace.WriteLine("将会尝试补充缺失资源... ", "INFO");

                List<Task> DownloadTasks = new List<Task>();
                Missingcontent.ForEach(item =>
                {
                    DownloadTasks.Add(Task.Run(() =>
                    {
                        DownloadUitl.DownloadAsync(item);
                        Trace.WriteLine("开始:" + item.Url + "  " + item.Size + "  " + item.Path, "INFO");
                    }));
                });
                await Task.WhenAll(DownloadTasks);
                Trace.WriteLine("所有资源下载结束", "INFO");
            }

            Launcher launcher = new Launcher(resolver, launchConfig);
            var watcher = await launcher.LaunchAsync(mainWindow.selectedGameEntry.Id);

            watcher.OutputLogReceived += (s, args) =>
            {
                Trace.WriteLine(args.Text, "GAMEINFO");
            };

            watcher.Exited += (s, arg) =>
            {
                mainWindow.Dispatcher.Invoke(() =>
                {
                    mainWindow.btn_StartGame.IsEnabled = true;
                    mainWindow.btn_StartGame.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(54, 170, 247));
                });
            };
        }



    }
}
