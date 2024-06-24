using MinecraftLaunch.Classes.Models.Auth;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
