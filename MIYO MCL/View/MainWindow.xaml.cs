using Microsoft.Windows.Themes;
using MinecraftLaunch.Classes.Interfaces;
using MinecraftLaunch.Classes.Models.Auth;
using MinecraftLaunch.Classes.Models.Launch;
using MinecraftLaunch.Components.Authenticator;
using MinecraftLaunch.Components.Launcher;
using MinecraftLaunch.Components.Resolver;
using MIYO_MCL.Class;
using MIYO_MCL.View;
using MIYO_Weather.Qweather;
using MIYO_Weather.Qweather.QweatherReceiveType;
using Panuon.WPF.UI;
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

#pragma warning disable CS8618

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
            AccountList.OfflineAccounts.Add(MIYO_BSMCL.CreateOfflineUser("QW1"));
            AccountList.OfflineAccounts.Add(MIYO_BSMCL.CreateOfflineUser("QW2"));
            AccountList.OfflineAccounts.Add(MIYO_BSMCL.CreateOfflineUser("QW3"));
            AccountList.OfflineAccounts.Add(MIYO_BSMCL.CreateOfflineUser("QW4"));
            AccountList.OfflineAccounts.Add(MIYO_BSMCL.CreateOfflineUser("QW5"));
            AccountList.OfflineAccounts.Add(MIYO_BSMCL.CreateOfflineUser("QW6"));
            mainWindowInit.BSMCLUserManager.SerializationUserToFile(AccountList, mainWindowInit.BSMCLUserManager.SafeModeStatus);
            

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
            selectedAccount = lb_UserList.SelectedItem;

            var configdata = mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindowInit.AppconfigManager.ReadConfigFile());
            configdata.LastSelectionUuid = ((Account)selectedAccount).Uuid.ToString();
            mainWindowInit.AppconfigManager.SerializationAndWriteConfigFile(configdata);
            tb_selectuserName.Text = ((Account)selectedAccount).Name;
        }

        private async void btn_delselectUser_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var result = MessageBoxX.Show(this,"确认要删除这个账号吗?","二次确认",MessageBoxButton.YesNo,MessageBoxIcon.Question,DefaultButton.NoCancel);

                if(result != MessageBoxResult.Yes)
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
            if(string.IsNullOrEmpty(tb_offlineName.Text))
            {
                Toast("玩家名不能为空");
                return;
            }
            AccountList.OfflineAccounts.Add(MIYO_BSMCL.CreateOfflineUser(tb_offlineName.Text));
            mainWindowInit.BSMCLUserManager.SerializationUserToFile(AccountList, mainWindowInit.BSMCLUserManager.SafeModeStatus);
            tb_offlineName.Text = string.Empty;
            Toast("创建成功");
            
        }
    }

    


}