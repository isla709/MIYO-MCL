using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;
using System.IO;
using MIYO_Weather;
using MIYO_Weather.Qweather;
using Newtonsoft.Json;
using MIYO_Weather.Qweather.QweatherReceiveType;
using System.Windows.Media.Imaging;
using MinecraftLaunch.Classes.Models.Download;
using MinecraftLaunch;

namespace MIYO_MCL.Class
{
    public class MIYO_Init
    {

        MainWindow mainWindow;
        public MIYO_ConfigManager AppconfigManager;
        public MIYO_SyncWeatherToControl SyncWeatherToControl;
        public MIYO_BSMCLUserManager BSMCLUserManager;



        public MIYO_Init(MainWindow mWindow) 
        {
            mainWindow = mWindow;
            AppconfigManager = new MIYO_ConfigManager();

            MIYO_LogTrace.InitLogTrace();
            AppconfigManager.VerifyFile();

            SyncWeatherToControl = new MIYO_SyncWeatherToControl(AppconfigManager) 
            {
                WeatherStatus = mainWindow.tb_Weather_Status,
                WeatherIcon = mainWindow.tb_Weather_Icon,
                WeatherTemp = mainWindow.tb_Weather_Temp,
                WeatherCity = mainWindow.tb_Weather_City,
            };

            BSMCLUserManager = new MIYO_BSMCLUserManager();

            

            mainWindow.OnApplicationEventBegin += MainWindow_OnApplicationEventBegin;
            mainWindow.OnApplicationEventEnd += MainWindow_OnApplicationEventEnd;
        }


        private void MainWindow_OnApplicationEventBegin()
        {
            AppConfigData appConfig = AppconfigManager.DeserializationAppConifgJson(AppconfigManager.ReadConfigFile());

            new MIYO_SyncSystemTimeToControl<TextBlock>(mainWindow.tb_SystemTime).Start();
            MIYO_CustomIMG.VerifyPath();
            SyncWeatherToControl.SyncToControl(mainWindow);
            BSMCLUserManager.SafeModeStatus = appConfig.AccountSaveMode == "1" ? false : true;
            BSMCLUserManager.VerifyFile(BSMCLUserManager.SafeModeStatus);

            MirrorDownloadManager.IsUseMirrorDownloadSource = true;

        }

        private void MainWindow_OnApplicationEventEnd()
        {
            MIYO_LogTrace.EndLogTrace();
        }



    }
}
