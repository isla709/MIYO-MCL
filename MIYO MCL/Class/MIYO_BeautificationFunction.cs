using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace MIYO_MCL.Class
{
    public static class MIYO_BeautificationFunction
    {
        public static void SetMainPageBackGround(MIYO_ConfigManager configManager, string? arg, MainWindow mainWindow)
        {
            Trace.WriteLine("修改主页背景为：" + arg, "INFO");

            if (string.IsNullOrEmpty(arg))
            {
                throw new Exception("SelectedValue Is Null.");
            }

            if (arg.Contains("Custom"))
            {
                mainWindow.comp_mainBackground.Visibility = Visibility.Visible;
                mainWindow.cb_mainBackground_Custom.IsEnabled = true;

                var configData = configManager.DeserializationAppConifgJson(configManager.ReadConfigFile());
                try 
                {
                    SetCustomImage(configManager, configData.CustomImage, mainWindow);
                }
                catch { }

                configData.BackgroundMain = "Custom";
                configManager.SerializationAndWriteConfigFile(configData);

                mainWindow.cb_mainBackground.SelectedItem = mainWindow.cb_mainBackground_item_Custom;
            }
            else if (arg.Contains("Theme"))
            {
                mainWindow.comp_mainBackground.Visibility = Visibility.Hidden;
                mainWindow.cb_mainBackground_Custom.IsEnabled = false;

                var configData = configManager.DeserializationAppConifgJson(configManager.ReadConfigFile());
                configData.BackgroundMain = "Theme";
                configManager.SerializationAndWriteConfigFile(configData);

                mainWindow.cb_mainBackground.SelectedItem = mainWindow.cb_mainBackground_item_Theme;

            }
            else
            {
                mainWindow.comp_mainBackground.Visibility = Visibility.Visible;
                mainWindow.cb_mainBackground_Custom.IsEnabled = false;

                string Imagepath = string.Format("pack://application:,,,/MIYO MCL;component/Asset/Image/{0}.png", arg);
                mainWindow.image_mainBackground.ImageSource = new BitmapImage(new Uri(Imagepath));

                var configData = configManager.DeserializationAppConifgJson(configManager.ReadConfigFile());
                configData.BackgroundMain = arg;
                configManager.SerializationAndWriteConfigFile(configData);

                Match match = Regex.Match(configData.BackgroundMain, @"\d+");
                if (match.Success)
                {
                    mainWindow.cb_mainBackground.SelectedIndex = int.Parse(match.Value) - 1;
                }

            }
        }

        public static void SetTheme(MIYO_ConfigManager configManager, string? arg, MainWindow mainWindow)
        {
            Trace.WriteLine("修改主题为：" + arg, "INFO");
            if (string.IsNullOrEmpty(arg))
            {
                throw new Exception("SelectedValue Is Null.");
            }

            if (arg.Contains("Light"))
            {
                List<SolidColorBrush> ThemeBrush = MIYO_Theme.GetAllThemeBrush();
                ThemeBrush[0].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_LightColor1);
                ThemeBrush[1].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_LightColor2);
                ThemeBrush[2].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_LightColor3);
                ThemeBrush[3].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_LightColor4);
                ThemeBrush[4].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_LightColor4);
                ThemeBrush[5].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_LightColor1);
                var configData = mainWindow.mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindow.mainWindowInit.AppconfigManager.ReadConfigFile());
                configData.Theme = arg;
                mainWindow.mainWindowInit.AppconfigManager.SerializationAndWriteConfigFile(configData);

                mainWindow.cb_Theme.SelectedItem = mainWindow.cb_Theme_Light;

            }
            else if (arg.Contains("Dark"))
            {

                List<SolidColorBrush> ThemeBrush = MIYO_Theme.GetAllThemeBrush();
                ThemeBrush[0].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_DarkColor1);
                ThemeBrush[1].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_DarkColor2);
                ThemeBrush[2].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_DarkColor3);
                ThemeBrush[3].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_DarkColor4);
                ThemeBrush[4].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_DarkColor4);
                ThemeBrush[5].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_DarkColor4);
                var configData = mainWindow.mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindow.mainWindowInit.AppconfigManager.ReadConfigFile());
                configData.Theme = arg;
                mainWindow.mainWindowInit.AppconfigManager.SerializationAndWriteConfigFile(configData);

                mainWindow.cb_Theme.SelectedItem = mainWindow.cb_Theme_Dark;
            }
            else if (arg.Contains("OceanicBliss"))
            {

                List<SolidColorBrush> ThemeBrush = MIYO_Theme.GetAllThemeBrush();
                ThemeBrush[0].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_OceanicBlissColor1);
                ThemeBrush[1].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_OceanicBlissColor2);
                ThemeBrush[2].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_OceanicBlissColor3);
                ThemeBrush[3].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_OceanicBlissColor4);
                ThemeBrush[4].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_OceanicBlissColor5);
                ThemeBrush[5].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_OceanicBlissColor5);
                var configData = mainWindow.mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindow.mainWindowInit.AppconfigManager.ReadConfigFile());
                configData.Theme = arg;
                mainWindow.mainWindowInit.AppconfigManager.SerializationAndWriteConfigFile(configData);

                mainWindow.cb_Theme.SelectedItem = mainWindow.cb_Theme_OceanicBliss;
            }

            else if (arg.Contains("SunsetGlow"))
            {
                List<SolidColorBrush> ThemeBrush = MIYO_Theme.GetAllThemeBrush();
                ThemeBrush[0].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_SunsetGlowColor1);
                ThemeBrush[1].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_SunsetGlowColor2);
                ThemeBrush[2].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_SunsetGlowColor3);
                ThemeBrush[3].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_SunsetGlowColor4);
                ThemeBrush[4].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_SunsetGlowColor5);
                ThemeBrush[5].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_SunsetGlowColor5);
                var configData = mainWindow.mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindow.mainWindowInit.AppconfigManager.ReadConfigFile());
                configData.Theme = arg;
                mainWindow.mainWindowInit.AppconfigManager.SerializationAndWriteConfigFile(configData);

                mainWindow.cb_Theme.SelectedItem = mainWindow.cb_Theme_SunsetGlow;
            }
            else if (arg.Contains("ForestHarmony"))
            {
                List<SolidColorBrush> ThemeBrush = MIYO_Theme.GetAllThemeBrush();
                ThemeBrush[0].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_ForestHarmonyColor1);
                ThemeBrush[1].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_ForestHarmonyColor2);
                ThemeBrush[2].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_ForestHarmonyColor3);
                ThemeBrush[3].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_ForestHarmonyColor4);
                ThemeBrush[4].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_ForestHarmonyColor5);
                ThemeBrush[5].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_ForestHarmonyColor6);
                var configData = mainWindow.mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindow.mainWindowInit.AppconfigManager.ReadConfigFile());
                configData.Theme = arg;
                mainWindow.mainWindowInit.AppconfigManager.SerializationAndWriteConfigFile(configData);

                mainWindow.cb_Theme.SelectedItem = mainWindow.cb_Theme_ForestHarmony;
            }
            else if(arg.Contains("CherryBlossom"))
            {
                List<SolidColorBrush> ThemeBrush = MIYO_Theme.GetAllThemeBrush();
                ThemeBrush[0].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_CherryBlossomColor1);
                ThemeBrush[1].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_CherryBlossomColor2);
                ThemeBrush[2].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_CherryBlossomColor3);
                ThemeBrush[3].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_CherryBlossomColor4);
                ThemeBrush[4].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_CherryBlossomColor5);
                ThemeBrush[5].Color = MIYO_Theme.GetThemeColor(ThemeColorName.Theme_CherryBlossomColor5);
                var configData = mainWindow.mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindow.mainWindowInit.AppconfigManager.ReadConfigFile());
                configData.Theme = arg;
                mainWindow.mainWindowInit.AppconfigManager.SerializationAndWriteConfigFile(configData);

                mainWindow.cb_Theme.SelectedItem = mainWindow.cb_Theme_CherryBlossom;
            }
            else
            {
                throw new Exception("SelectedValue Is invalid");
            }

        }

        public static void LoadCustomImage(MainWindow mainWindow) 
        {
            var CustomImageList = MIYO_CustomIMG.SacnImageFile(MIYO_CustomIMG.CustomImagePath);
            CustomImageList.ForEach(image => mainWindow.cb_mainBackground_Custom.Items.Add(image));


        }

        public static void SetCustomImage(MIYO_ConfigManager configManager, string? path, MainWindow mainWindow)
        {

            if (string.IsNullOrEmpty(path))
            {
                throw new Exception("path Is Null.");
            }
            if (path.Equals("NULL"))
            {
                throw new Exception("path Is Null.");
            }

            if (mainWindow.cb_mainBackground_Custom.IsEnabled)
            {
                mainWindow.image_mainBackground.ImageSource = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));


                var configData = mainWindow.mainWindowInit.AppconfigManager.DeserializationAppConifgJson(mainWindow.mainWindowInit.AppconfigManager.ReadConfigFile());
                configData.CustomImage = path;
                mainWindow.mainWindowInit.AppconfigManager.SerializationAndWriteConfigFile(configData);
            }

        }

    }
}
