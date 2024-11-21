using Microsoft.Windows.Themes;
using MinecraftLaunch;
using MinecraftLaunch.Classes.Interfaces;
using MinecraftLaunch.Classes.Models.Auth;
using MinecraftLaunch.Classes.Models.Game;
using MinecraftLaunch.Classes.Models.Install;
using MinecraftLaunch.Classes.Models.Launch;
using MinecraftLaunch.Components.Authenticator;
using MinecraftLaunch.Components.Checker;
using MinecraftLaunch.Components.Downloader;
using MinecraftLaunch.Components.Fetcher;
using MinecraftLaunch.Components.Installer;
using MinecraftLaunch.Components.Launcher;
using MinecraftLaunch.Components.Resolver;
using MinecraftLaunch.Utilities;
using MIYO_MCL.Class;
using MIYO_MCL.View;
using MIYO_Weather.Qweather;
using MIYO_Weather.Qweather.QweatherReceiveType;
using Panuon.WPF.UI;
using System.Collections.Frozen;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MinecraftLaunch.Classes.Models.Download;

#pragma warning disable CS8618
#pragma warning disable CS8601

namespace MIYO_MCL
{
    public delegate void MIYO_OnApplicationEventBeginHandler();

    public delegate void MIYO_OnApplicationEventEndHandler();



    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowX
    {

        public event MIYO_OnApplicationEventBeginHandler? OnApplicationEventBegin;

        public event MIYO_OnApplicationEventEndHandler? OnApplicationEventEnd;

        public MIYO_Init mainWindowInit;

        public QweatherAPI qweatherAPI;

        public QW_WeatherNowData? weatherNowData;

        public QW_CityFindData cityFindData;

        public AccountData AccountList;

        public object selectedAccount;

        public GameEntry selectedGameEntry;

        public VersionManifestEntry selectedInstallVersion;

        public GameEntry selectedInstallForgeGameEntry;

        public UInt16 installForgePageNum = 0;

        public GameEntry selectedInstallFabricGameEntry;

        public UInt16 installFabricPageNum = 0;

        public GameEntry selectedInstallQuiltGameEntry;
        
        public UInt16 installQuiltPageNum = 0;

        public MainWindow()

        {
            InitializeComponent();


            this.Hide();
            new StartupWindow(this).Show();

            mainWindowInit = new MIYO_Init(this);
            OnApplicationEventBegin?.Invoke();
        }

        private void WindowX_Closed(object sender, EventArgs e)
        {

            OnApplicationEventEnd?.Invoke();
        }

        private void WindowX_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {

                var configData = mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindowInit.AppconfigManager.ReadConfigFile());
                MIYO_BeautificationFunction.SetMainPageBackGround(mainWindowInit.AppconfigManager, configData.BackgroundMain, this);
                MIYO_BeautificationFunction.LoadCustomImage(this);
                MIYO_BeautificationFunction.SetTheme(mainWindowInit.AppconfigManager, configData.Theme, this);

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }


            try
            {
                var configData = mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindowInit.AppconfigManager.ReadConfigFile());

                IGameResolver resolver;
                if (configData.GamePath.Equals("SoftwareDirectory"))
                {
                    resolver = new GameResolver(".minecraft");
                }
                else
                {
                    resolver = new GameResolver(configData.GamePath);
                }

                if (resolver == null)
                {
                    throw new Exception("resolver Not Init");
                }

                List<GameEntry> gameGameEntrys = resolver.GetGameEntitys().ToList();
                gameGameEntrys.ForEach(game =>
                {
                    if (configData.LastSelectionVersion.Equals(game.Id))
                    {
                        selectedGameEntry = game;
                        tb_GameVision.Text = selectedGameEntry.Id;
                        tb_selectverName.Text = selectedGameEntry.Id;
                    }

                });
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }


        }

        private void cb_mainBackground_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string? SelectedName = ((ComboBoxItem)cb_mainBackground.SelectedValue).Content.ToString();
                MIYO_BeautificationFunction.SetMainPageBackGround(mainWindowInit.AppconfigManager, SelectedName, this);

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }

        }

        private void cb_Theme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                string? SelectedName = ((ComboBoxItem)cb_Theme.SelectedValue).Content.ToString();
                MIYO_BeautificationFunction.SetTheme(mainWindowInit.AppconfigManager, SelectedName, this);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());

            }

        }

        private void cb_mainBackground_Custom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? SelectedName = (string)cb_mainBackground_Custom.SelectedValue;
            MIYO_BeautificationFunction.SetCustomImage(mainWindowInit.AppconfigManager, SelectedName, this);
        }

        private void comp_WeatherSettings_Loaded(object sender, RoutedEventArgs e)
        {

            Task.Run(async () =>
            {
                while (true)
                {
                    if (weatherNowData != null)
                    {
                        await Dispatcher.BeginInvoke(() =>
                        {
                            try
                            {
                                tb_CityName.Text = cityFindData.location[0].adm1 + " " + cityFindData.location[0].name;
                                tb_CityID.Text = cityFindData.location[0].id;
                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLine(ex.Message);
                            }


                        });

                        break;
                    }
                    await Task.Delay(TimeSpan.FromSeconds(0.5));
                }


            });
        }

        private void btn_ChangeCity_Click(object sender, RoutedEventArgs e)
        {
            FindCityWindow findCityWindow = new FindCityWindow(mainWindowInit.SyncWeatherToControl.GetQweatherAPI());
            findCityWindow.OnFindCityWindowFinish += citydata =>
            {
                var configdata = mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindowInit.AppconfigManager.ReadConfigFile());
                configdata.CityID = citydata.id;
                mainWindowInit.AppconfigManager.SerializationAndWriteConfigFile(configdata);

                weatherNowData = null;

                mainWindowInit.SyncWeatherToControl.SyncToControl(this);

                Task.Run(async () =>
                {
                    while (true)
                    {
                        if (weatherNowData != null)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                tb_CityName.Text = cityFindData.location[0].adm1 + " " + cityFindData.location[0].name;
                                tb_CityID.Text = cityFindData.location[0].id;

                            });

                            break;
                        }
                        await Task.Delay(TimeSpan.FromSeconds(0.5));
                    }


                });


            };
            findCityWindow.Show();
        }

        private void btn_StartGame_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                MIYO_BSMCLFunction.StartGame(this);
            }
            catch(Exception ex) 
            {
                Trace.WriteLine(ex);
                MessageBoxX.Show(this,"启动失败，详情查看日志","错误",MessageBoxButton.OK,MessageBoxIcon.Error);
            }
           
        }


        private void btn_switchTover1_Click(object sender, RoutedEventArgs e)
        {
            tc_leftplane.SelectedIndex = 1;
        }

        private void cb_AccountSaveMode_Loaded(object sender, RoutedEventArgs e)
        {
            var configdata = mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindowInit.AppconfigManager.ReadConfigFile());
            cb_AccountSaveMode.SelectedIndex = int.Parse(configdata.AccountSaveMode);
        }

        private void cb_AccountSaveMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var configdata = mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindowInit.AppconfigManager.ReadConfigFile());
            configdata.AccountSaveMode = cb_AccountSaveMode.SelectedIndex.ToString();
            mainWindowInit.AppconfigManager.SerializationAndWriteConfigFile(configdata);

        }

        private void btn_AccountSaveMode_info_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxX.Show(this, "使用系统用户文件夹模式时，账户信息将存放于：C:\\Users\\<User name>\\Documents\\MIYOMCL \n 使用软件目录下模式时，账户信息将存放于：.\\MIYOMCL\\Account", "Tips", MessageBoxIcon.Info, DefaultButton.YesOK);
        }

        private void lb_UserList_Loaded(object sender, RoutedEventArgs e)
        {
            MIYO_BSMCLFunction.LoadUserList(this);
        }

        private void btn_changeselectUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (lb_UserList.SelectedIndex == -1)
                {
                    Toast("请先选择用户");
                    return;
                }

                selectedAccount = lb_UserList.SelectedItem;

                var configdata = mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindowInit.AppconfigManager.ReadConfigFile());
                configdata.LastSelectionUuid = ((Account)selectedAccount).Uuid.ToString();
                mainWindowInit.AppconfigManager.SerializationAndWriteConfigFile(configdata);
                tb_selectuserName.Text = ((Account)selectedAccount).Name;
            }
            catch(Exception ex) 
            {    
                Trace.WriteLine(ex);
            }
        }

        private async void btn_delselectUser_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var result = MessageBoxX.Show(this, "确认要删除这个账号吗?", "二次确认", MessageBoxButton.YesNo, MessageBoxIcon.Question, DefaultButton.NoCancel);

                if (result != MessageBoxResult.Yes)
                {
                    return;
                }

                var configdata = mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindowInit.AppconfigManager.ReadConfigFile());

                var userdata = await MIYO_BSMCLFunction.GetUserData(this);
                AccountList = userdata;

                if (lb_UserList.SelectedItems != null)
                {

                    if (selectedAccount != null)
                    {
                        if (((Account)selectedAccount).Uuid == ((Account)lb_UserList.SelectedItem).Uuid)
                        {

                            configdata.LastSelectionUuid = "";
                            tb_selectuserName.Text = "";
                            mainWindowInit.AppconfigManager.SerializationAndWriteConfigFile(configdata);
                        }
                    }

                    if (AccountList.MicrosoftAccounts != null)
                    {
                        AccountList.MicrosoftAccounts.RemoveAll(account => account.AccessToken == ((Account)lb_UserList.SelectedItem).AccessToken);
                    }

                    if (AccountList.OfflineAccounts != null)
                    {
                        AccountList.OfflineAccounts.RemoveAll(account => account.AccessToken == ((Account)lb_UserList.SelectedItem).AccessToken);
                    }

                    if (AccountList.YggdrasilAccounts != null)
                    {
                        AccountList.YggdrasilAccounts.RemoveAll(account => account.AccessToken == ((Account)lb_UserList.SelectedItem).AccessToken);
                    }

                    mainWindowInit.BSMCLUserManager.SerializationUserToFile(AccountList, mainWindowInit.BSMCLUserManager.SafeModeStatus);
                    await Task.Delay(500);
                    MIYO_BSMCLFunction.LoadUserList(this);
                }

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }

        }

        private async void btn_RunCreateMicrosoft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var microsoftAccount = await MIYO_BSMCL.CreateMicrosoftUser(this);
                AccountList.MicrosoftAccounts.Add(microsoftAccount);
                mainWindowInit.BSMCLUserManager.SerializationUserToFile(AccountList, mainWindowInit.BSMCLUserManager.SafeModeStatus);
                Toast("创建成功");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }

        }

        private void btn_RunCreateOffline_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tb_offlineName.Text))
            {
                Toast("玩家名不能为空");
                return;
            }
            AccountList.OfflineAccounts.Add(MIYO_BSMCL.CreateOfflineUser(tb_offlineName.Text));
            mainWindowInit.BSMCLUserManager.SerializationUserToFile(AccountList, mainWindowInit.BSMCLUserManager.SafeModeStatus);
            tb_offlineName.Text = string.Empty;
            Toast("创建成功");

        }

        private void cb_GamePath_Loaded(object sender, RoutedEventArgs e)
        {
            var configdata = mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindowInit.AppconfigManager.ReadConfigFile());
            if (configdata.GamePath.Equals("SoftwareDirectory"))
            {
                cb_GamePath.SelectedIndex = 0;
            }


        }

        private void cb_GamePath_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var configdata = mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindowInit.AppconfigManager.ReadConfigFile());
            switch (cb_GamePath.SelectedIndex)
            {
                case 0:
                    configdata.GamePath = "SoftwareDirectory";
                    mainWindowInit.AppconfigManager.SerializationAndWriteConfigFile(configdata);
                    break;

                default:
                    break;

            }
        }

        private void lb_verList_Loaded(object sender, RoutedEventArgs e)
        {
            IGameResolver resolver = MIYO_BSMCLFunction.GetMIYOMCLGameResolver(this);

            List<GameEntry> games = resolver.GetGameEntitys().ToList();
            lb_verList.Items.Clear();
            games.ForEach(g => { lb_verList.Items.Add(g); });
            


            
        }

        private void btn_changeselectver_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lb_verList.SelectedIndex == -1)
                {
                    Toast("请先选择版本");
                    return;
                }

                selectedGameEntry = ((GameEntry)lb_verList.SelectedItem);

                var configdata = mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindowInit.AppconfigManager.ReadConfigFile());
                configdata.LastSelectionVersion = selectedGameEntry.Id;
                mainWindowInit.AppconfigManager.SerializationAndWriteConfigFile(configdata);
                tb_GameVision.Text = selectedGameEntry.Id;
                tb_selectverName.Text = selectedGameEntry.Id;

            }
            catch (Exception ex) 
            {
                Trace.WriteLine(ex);
            }



        }

        private async void cb_javapath_Loaded(object sender, RoutedEventArgs e)
        {
            var configdata = mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindowInit.AppconfigManager.ReadConfigFile());

            JavaFetcher javaFetcher = new JavaFetcher();
            var Javas = await javaFetcher.FetchAsync();
            var JavaList = Javas.ToList();
            JavaList.ForEach(java => 
            {
                Trace.WriteLine($"发现Java：{java.JavaPath}  {java.JavaVersion} {java.JavaSlugVersion}  64位：{java.Is64Bit}");
                cb_javapath.Items.Add(java);
                if (configdata.JavaPath.Equals(java.JavaPath)) 
                {
                    cb_javapath.SelectedItem = java;
                }
            });
        }

        private void cb_javapath_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var configdata = mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindowInit.AppconfigManager.ReadConfigFile());
            configdata.JavaPath = ((JavaEntry)cb_javapath.SelectedItem).JavaPath;
            mainWindowInit.AppconfigManager.SerializationAndWriteConfigFile(configdata);
        }

        private void ni_jvmmMax_Loaded(object sender, RoutedEventArgs e)
        {
            var configdata = mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindowInit.AppconfigManager.ReadConfigFile());
            ni_jvmmMax.Value = int.Parse(configdata.JvmMaxMemory);
        }

        private void ni_jvmmMin_Loaded(object sender, RoutedEventArgs e)
        {
            var configdata = mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindowInit.AppconfigManager.ReadConfigFile());
            ni_jvmmMin.Value = int.Parse(configdata.JvmMinMemory);
        }

        private void ni_jvmmMax_ValueChanged(object sender, Panuon.WPF.SelectedValueChangedRoutedEventArgs<double?> e)
        {
            var configdata = mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindowInit.AppconfigManager.ReadConfigFile());
            if(ni_jvmmMax.Value != null)
            {
                configdata.JvmMaxMemory = ni_jvmmMax.Value.ToString();
                mainWindowInit.AppconfigManager.SerializationAndWriteConfigFile(configdata);
            }
            
        }

        private void ni_jvmmMin_ValueChanged(object sender, Panuon.WPF.SelectedValueChangedRoutedEventArgs<double?> e)
        {
            var configdata = mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindowInit.AppconfigManager.ReadConfigFile());
            if (ni_jvmmMin.Value != null)
            {
                configdata.JvmMinMemory = ni_jvmmMin.Value.ToString();
                mainWindowInit.AppconfigManager.SerializationAndWriteConfigFile(configdata);
            }
        }

        private async void lb_installVanllia_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                lb_installVanilla.Items.Clear();
                (await VanlliaInstaller.EnumerableGameCoreAsync()).ToList().ForEach(ver => lb_installVanilla.Items.Add(ver));
            }
            catch (Exception ex) 
            {
                MessageBox.Show(this,ex.Message);
                Trace.WriteLine(ex.Message);
            }
            
        
        }

        private void lb_installVanllia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try 
            {
                if (lb_installVanilla.SelectedItem is VersionManifestEntry)
                {
                    selectedInstallVersion = lb_installVanilla.SelectedItem as VersionManifestEntry;
                    if (selectedInstallVersion != null)
                    {
                        tb_selectedInstall.Text = selectedInstallVersion.Id;
                    }
                    else
                    {
                        Toast("未知错误");
                    }
                }
                else 
                {
                
                }
                
            }
            catch (Exception ex)
            {
                MessageBoxX.Show(ex.Message, "发生了一个错误", MessageBoxButton.OK, MessageBoxIcon.Error);
            }
            
        }

        private async void btn_installver_Click(object sender, RoutedEventArgs e)
        {
            IPendingHandler installPendingBox = null;
            try
            {
                VanlliaInstaller installer = new VanlliaInstaller(MIYO_BSMCLFunction.GetMIYOMCLGameResolver(this), tb_selectedInstall.Text, MirrorDownloadManager.Bmcl);
                installPendingBox = PendingBox.Show(this,"正在安装" + tb_selectedInstall.Text + " ", "原版安装器", false);
                installer.ProgressChanged += (_, e) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        installPendingBox.UpdateMessage("正在安装" + tb_selectedInstall.Text + ": " + e.ProgressStatus);
                    });

                    Trace.WriteLine(e.ProgressStatus, "[原版安装器]");

                };
                installer.Completed += async (_, e) =>
                {
                    installPendingBox.Close();
                    Toast("安装完成");
                    
                    var pendingHandler = PendingBox.Show(this, "正在查找缺失资源。", "资源检查器", false);
                    
                    IGameResolver resolver = MIYO_BSMCLFunction.GetMIYOMCLGameResolver(this);

                    List<GameEntry> games = resolver.GetGameEntitys().ToList();
                    
                    GameEntry checktarget = null;
                    games.ForEach(g =>
                    {
                        if (tb_selectedInstall.Text == g.Id)
                        {
                            checktarget = g;
                        }
                        
                    });
                    if (checktarget == null)
                    {
                        pendingHandler.Close();
                        return;
                    }
                    ResourceChecker resourceChecker = new ResourceChecker(checktarget);
                    if (!await resourceChecker.CheckAsync()) 
                    {
                        pendingHandler.UpdateMessage($"已发现缺失:{resourceChecker.MissingResources.Count}");
                        await Task.Delay(500);
                        DownloadRequest request = new DownloadRequest() 
                        {
                            FileSizeThreshold = 50 * 1024 * 1024, // 阈值为 50MB
                            MultiPartsCount = 4, // 分为4个部分下载
                            MultiThreadsCount = 4, // 使用4个线程并发下载
                            IsPartialContentSupported = true // 支持断点续传
                        };
                        ResourceDownloader resourceDownloader = new ResourceDownloader(request, resourceChecker.MissingResources,MirrorDownloadManager.Bmcl);
                        resourceDownloader.ProgressChanged += (_,e) => 
                        {
                            this.Dispatcher.Invoke(() => 
                            {
                                pendingHandler.UpdateMessage($"资源下载中:{e.CompletedCount}/{e.TotalCount}");
                            });
                    
                        };
                        await resourceDownloader.DownloadAsync();
                        this.Toast("资源下载结束");
                    }
                    pendingHandler.Close();
                    
                    
                };
                await installer.InstallAsync();
            }
            catch (Exception ex) 
            {
                Trace.WriteLine(ex.Message, "Error");
                if (installPendingBox != null)
                {
                    installPendingBox.Close();
                }
                MessageBox.Show(this,"安装失败", "原版安装器");

            }


        }

        private void lb_installForge_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_forgeinstall.IsEnabled = false;
                (sender as ListBox)?.Items.Clear();

                IGameResolver resolver = MIYO_BSMCLFunction.GetMIYOMCLGameResolver(this);
                List<GameEntry> games = resolver.GetGameEntitys().ToList();
                games.ForEach(game => { (sender as ListBox)?.Items.Add(game); });


                installForgePageNum = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
                Trace.WriteLine(ex.Message);
            }
        }

        private void btn_forgeinstall_back_Click(object sender, RoutedEventArgs e)
        {

            lb_installForge_Loaded(lb_installForge, e);
            
        }

        private async void lb_installForge_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var c_owner = sender as ListBox;
            if (c_owner == null) 
            {
                MessageBox.Show(this, "未知错误！！！"); 
                return; 
            }
            if(installForgePageNum == 0)
            {
                selectedInstallForgeGameEntry = (c_owner.SelectedItem as GameEntry);
                if (selectedInstallForgeGameEntry == null) { return; }
                var vername = selectedInstallForgeGameEntry.Id;
                var ForgeResult = (await ForgeInstaller.EnumerableFromVersionAsync(vername)).ToList();
                c_owner.Items.Clear();
                ForgeResult.ForEach(forgeinfo => c_owner.Items.Add(forgeinfo));
                installForgePageNum = 1;
                btn_forgeinstall.IsEnabled = true;
            }
        }

        private void lb_installForge_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var c_owner = sender as ListBox;
            if (c_owner == null)
            {
                MessageBox.Show(this, "未知错误！！！");
                return;
            }
            if (installForgePageNum == 1)
            {
                var Selectedforgever = (c_owner.SelectedItem as ForgeInstallEntry);
                if (Selectedforgever != null)
                {
                    tb_selectedforgeVer.Text = string.Format($"{Selectedforgever.ForgeVersion}  -{Selectedforgever.McVersion}");
                }

                
            }
        }

        private async void btn_forgeinstall_Click(object sender, RoutedEventArgs e)
        {
            var configdata = mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindowInit.AppconfigManager.ReadConfigFile());
            IPendingHandler installPendingBox = null;
            if (installForgePageNum == 1)
            {
                
                try 
                {
                                    
                    var Selectedforgever = (lb_installForge.SelectedItem as ForgeInstallEntry);
                    if (Selectedforgever == null)
                    {
                        return;
                    }
                    installPendingBox = PendingBox.Show(this,"正在安装" + tb_selectedforgeVer.Text + " ", "Forge安装器", false);
                    ForgeInstaller installer = new ForgeInstaller(selectedInstallForgeGameEntry, Selectedforgever, configdata.JavaPath,tb_selectedforgeVer.Text,MirrorDownloadManager.Bmcl);
                    installer.ProgressChanged += (_, e) =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            installPendingBox.UpdateMessage("正在安装" + tb_selectedforgeVer.Text + ": " + e.ProgressStatus);
                        });
                        Trace.WriteLine(e.ProgressStatus, "[Forge安装器]");
                    };
                    installer.Completed += (_, e) =>
                    {
                        Toast("安装完成");
                        Trace.WriteLine("安装完成","INFO");
                    };

                    MirrorDownloadManager.IsUseMirrorDownloadSource = false;
                    await installer.InstallAsync();
                    MirrorDownloadManager.IsUseMirrorDownloadSource = true;
                    
                    installPendingBox.Close();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex, "Error");
                    if (installPendingBox != null)
                    {
                        installPendingBox.Close();
                    }
                    MessageBoxX.Show(this, $"安装失败:{ex.Message}", "Forge安装器");

                }



            }
        }

        private void lb_installFabric_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_fabricinstall.IsEnabled = false;
                (sender as ListBox)?.Items.Clear();

                IGameResolver resolver = MIYO_BSMCLFunction.GetMIYOMCLGameResolver(this);
                List<GameEntry> games = resolver.GetGameEntitys().ToList();
                games.ForEach(game => { (sender as ListBox)?.Items.Add(game); });


                installFabricPageNum = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
                Trace.WriteLine(ex.Message);
            }
        }

        private void btn_fabricinstall_back_Click(object sender, RoutedEventArgs e)
        {
            lb_installFabric_Loaded(lb_installFabric, e);
        }

        private async void lb_installFabric_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var c_owner = sender as ListBox;
            if (c_owner == null)
            {
                MessageBox.Show(this, "未知错误！！！");
                return;
            }
            if (installFabricPageNum == 0)
            {
                selectedInstallFabricGameEntry = (c_owner.SelectedItem as GameEntry);
                if (selectedInstallFabricGameEntry == null) { return; }
                var vername = selectedInstallFabricGameEntry.Id;
                var FabricResult = (await FabricInstaller.EnumerableFromVersionAsync(vername)).ToList();
                c_owner.Items.Clear();
                FabricResult.ForEach(fabricinfo => c_owner.Items.Add(fabricinfo));
                installFabricPageNum = 1;
                btn_fabricinstall.IsEnabled = true;
            }
        }

        private void lb_installFabric_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var c_owner = sender as ListBox;
            if (c_owner == null)
            {
                MessageBox.Show(this, "未知错误！！！");
                return;
            }
            if (installFabricPageNum == 1)
            {
                var Selectedfabricver = (c_owner.SelectedItem as FabricBuildEntry);
                if (Selectedfabricver != null)
                {
                    tb_selectedfabricVer.Text = string.Format($"{Selectedfabricver.DisplayVersion}");
                }


            }
        }

        private async void btn_fabricinstall_Click(object sender, RoutedEventArgs e)
        {
            var configdata = mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindowInit.AppconfigManager.ReadConfigFile());
            IPendingHandler installPendingBox = null;
            if (installFabricPageNum == 1)
            {
                try
                {
                    var Selectedfabricver = (lb_installFabric.SelectedItem as FabricBuildEntry);
                    if (Selectedfabricver == null)
                    {
                        return;
                    }
                    installPendingBox = PendingBox.Show(this, "正在安装" + tb_selectedfabricVer.Text + " ", "Fabric安装器", false);
                    FabricInstaller installer = new FabricInstaller(selectedInstallFabricGameEntry, Selectedfabricver, tb_selectedfabricVer.Text, MirrorDownloadManager.Bmcl);
                    installer.ProgressChanged += (_, e) =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            installPendingBox.UpdateMessage("正在安装" + tb_selectedfabricVer.Text + ": " + e.ProgressStatus);
                        });
                        Trace.WriteLine(e.ProgressStatus, "[Fabric安装器]");
                    };
                    installer.Completed += (_, e) =>
                    {
                        
                        Toast("安装完成");
                        Trace.WriteLine("安装完成", "INFO");
                    };
                    await installer.InstallAsync();
                    installPendingBox.Close();
                    
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message, "Error");
                    if (installPendingBox != null)
                    {
                        installPendingBox.Close();
                    }
                    MessageBoxX.Show(this, $"安装失败:{ex.Message}", "Fabric安装器");

                }



            }
        }

        private async void Btn_resourceChecker_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lb_verList.SelectedIndex == -1)
                {
                    Toast("请先选择版本");
                    return;
                }
                
                var pendingHandler = PendingBox.Show(this, "正在查找缺失资源。", "资源检查器", false);
                ResourceChecker resourceChecker = new ResourceChecker(((GameEntry)lb_verList.SelectedItem));
                if (!await resourceChecker.CheckAsync()) 
                {
                    pendingHandler.UpdateMessage($"已发现缺失:{resourceChecker.MissingResources.Count}");
                    await Task.Delay(500);
                    DownloadRequest request = new DownloadRequest() 
                    {
                        FileSizeThreshold = 50 * 1024 * 1024, // 阈值为 50MB
                        MultiPartsCount = 4, // 分为4个部分下载
                        MultiThreadsCount = 4, // 使用4个线程并发下载
                        IsPartialContentSupported = true // 支持断点续传
                    };
                    ResourceDownloader resourceDownloader = new ResourceDownloader(request, resourceChecker.MissingResources,MirrorDownloadManager.Bmcl);
                    resourceDownloader.ProgressChanged += (_,e) => 
                    {
                        Dispatcher.Invoke(() => 
                        {
                            pendingHandler.UpdateMessage($"资源下载中:{e.CompletedCount}/{e.TotalCount}");
                        });
                    
                    };
                    await resourceDownloader.DownloadAsync();
                    Toast("资源下载结束");
                }
                pendingHandler.Close();
                

            }
            catch (Exception ex) 
            {
                Trace.WriteLine(ex);
            }
            
            
            
        }

        private void lb_installQuilt_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_Quiltinstall.IsEnabled = false;
                (sender as ListBox)?.Items.Clear();

                IGameResolver resolver = MIYO_BSMCLFunction.GetMIYOMCLGameResolver(this);
                List<GameEntry> games = resolver.GetGameEntitys().ToList();
                games.ForEach(game => { (sender as ListBox)?.Items.Add(game); });


                installQuiltPageNum = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
                Trace.WriteLine(ex.Message);
            }
        }

        private async void Lb_installQuilt_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var c_owner = sender as ListBox;
            if (c_owner == null)
            {
                MessageBox.Show(this, "未知错误！！！");
                return;
            }
            if (installQuiltPageNum == 0)
            {
                selectedInstallQuiltGameEntry = (c_owner.SelectedItem as GameEntry);
                if (selectedInstallQuiltGameEntry == null) { return; }
                var vername = selectedInstallQuiltGameEntry.Id;
                var QuiltResult = (await QuiltInstaller.EnumerableFromVersionAsync(vername)).ToList();
                c_owner.Items.Clear();
                QuiltResult.ForEach(quiltinfo => c_owner.Items.Add(quiltinfo));
                installQuiltPageNum = 1;
                btn_Quiltinstall.IsEnabled = true;
            }
        }

        private void Btn_Quiltinstall_back_OnClick(object sender, RoutedEventArgs e)
        {
            lb_installQuilt_OnLoaded(lb_installQuilt, e);
        }

        private void Lb_installQuilt_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var c_owner = sender as ListBox;
            if (c_owner == null)
            {
                MessageBox.Show(this, "未知错误！！！");
                return;
            }
            if (installQuiltPageNum == 1)
            {
                var SelectedQuiltver = (c_owner.SelectedItem as QuiltBuildEntry);
                if (SelectedQuiltver != null)
                {
                    tb_selectedQuiltVer.Text = string.Format($"{SelectedQuiltver.DisplayVersion}");
                }


            }
        }

        private async void Btn_Quiltinstall_OnClick(object sender, RoutedEventArgs e)
        {
            var configdata = mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindowInit.AppconfigManager.ReadConfigFile());
            IPendingHandler installPendingBox = null;
            if (installQuiltPageNum == 1)
            {
                try
                {
                    var SelectedQuiltver = (lb_installQuilt.SelectedItem as QuiltBuildEntry);
                    if (SelectedQuiltver == null)
                    {
                        return;
                    }
                    installPendingBox = PendingBox.Show(this, "正在安装" + tb_selectedQuiltVer.Text + " ", "Quilt安装器", false);
                    QuiltInstaller installer = new QuiltInstaller(selectedInstallQuiltGameEntry, SelectedQuiltver, tb_selectedQuiltVer.Text, MirrorDownloadManager.Bmcl);
                    installer.ProgressChanged += (_, e) =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            installPendingBox.UpdateMessage("正在安装" + tb_selectedQuiltVer.Text + ": " + e.ProgressStatus);
                        });
                        Trace.WriteLine(e.ProgressStatus, "[Quilt安装器]");
                    };
                    installer.Completed += (_, e) =>
                    {
                        
                        Toast("安装完成");
                        Trace.WriteLine("安装完成", "INFO");
                    };
                    await installer.InstallAsync();
                    installPendingBox.Close();
                    
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message, "Error");
                    if (installPendingBox != null)
                    {
                        installPendingBox.Close();
                    }
                    MessageBoxX.Show(this, $"安装失败:{ex.Message}", "Quilt安装器");

                }



            }
        }
    }

    

    public class InstallForgeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate IF0 { get; set; }
        public DataTemplate IF1 { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var entry = item as GameEntry; // 替换为实际的数据模型类型

            // 根据是否存在 Id 决定使用哪个模板
            if (!string.IsNullOrEmpty(entry?.Id))
            {
                return IF0;
            }
            else
            {
                return IF1;
            }
        }
    }

    public class InstallFabricTemplateSelector : DataTemplateSelector
    {
        public DataTemplate IF0 { get; set; }
        public DataTemplate IF1 { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var entry = item as GameEntry; // 替换为实际的数据模型类型

            // 根据是否存在 Id 决定使用哪个模板
            if (!string.IsNullOrEmpty(entry?.Id))
            {
                return IF0;
            }
            else
            {
                return IF1;
            }
        }
    }
    
    public class InstallQuiltTemplateSelector : DataTemplateSelector
    {
        public DataTemplate IF0 { get; set; }
        public DataTemplate IF1 { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var entry = item as GameEntry; // 替换为实际的数据模型类型

            // 根据是否存在 Id 决定使用哪个模板
            if (!string.IsNullOrEmpty(entry?.Id))
            {
                return IF0;
            }
            else
            {
                return IF1;
            }
        }
    }
}