using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MIYO_MCL.Class
{
    public struct ThemeBrushName
    {
        public static string Brush_MainBackGround = "ThemeBrush_MainBackGround";
        public static string Brush_CompBackGround = "ThemeBrush_CompBackGround";
        public static string Brush_UnSelectedColor = "ThemeBrush_UnSelectedColor";
        public static string Brush_SelectedColor = "ThemeBrush_SelectedColor";
        public static string Brush_FontColor1 = "ThemeBrush_FontColor1";
        public static string Brush_FontColor2 = "ThemeBrush_FontColor2";

    }

    public struct ThemeColorName
    {
        public static string Theme_LightColor1 = "Theme_LightColor1";
        public static string Theme_LightColor2 = "Theme_LightColor2";
        public static string Theme_LightColor3 = "Theme_LightColor3";
        public static string Theme_LightColor4 = "Theme_LightColor4";

        public static string Theme_DarkColor1 = "Theme_DarkColor1";
        public static string Theme_DarkColor2 = "Theme_DarkColor2";
        public static string Theme_DarkColor3 = "Theme_DarkColor3";
        public static string Theme_DarkColor4 = "Theme_DarkColor4";

        public static string Theme_OceanicBlissColor1 = "Theme_OceanicBlissColor1"; // MainBackGround
        public static string Theme_OceanicBlissColor2 = "Theme_OceanicBlissColor2"; // CompBackGround
        public static string Theme_OceanicBlissColor3 = "Theme_OceanicBlissColor3"; // UnSelectedColor
        public static string Theme_OceanicBlissColor4 = "Theme_OceanicBlissColor4"; // SelectedColor
        public static string Theme_OceanicBlissColor5 = "Theme_OceanicBlissColor5"; // FontColor

        public static string Theme_SunsetGlowColor1 = "Theme_SunsetGlowColor1"; // MainBackGround
        public static string Theme_SunsetGlowColor2 = "Theme_SunsetGlowColor2"; // CompBackGround
        public static string Theme_SunsetGlowColor3 = "Theme_SunsetGlowColor3"; // UnSelectedColor
        public static string Theme_SunsetGlowColor4 = "Theme_SunsetGlowColor4"; // SelectedColor
        public static string Theme_SunsetGlowColor5 = "Theme_SunsetGlowColor5"; // FontColor

        public static string Theme_ForestHarmonyColor1 = "Theme_ForestHarmonyColor1"; // MainBackGround
        public static string Theme_ForestHarmonyColor2 = "Theme_ForestHarmonyColor2"; // CompBackGround
        public static string Theme_ForestHarmonyColor3 = "Theme_ForestHarmonyColor3"; // UnSelectedColor
        public static string Theme_ForestHarmonyColor4 = "Theme_ForestHarmonyColor4"; // SelectedColor
        public static string Theme_ForestHarmonyColor5 = "Theme_ForestHarmonyColor5"; // FontColor
        public static string Theme_ForestHarmonyColor6 = "Theme_ForestHarmonyColor6"; // FontColor

        public static string Theme_CherryBlossomColor1 = "Theme_CherryBlossomColor1"; // MainBackGround
        public static string Theme_CherryBlossomColor2 = "Theme_CherryBlossomColor2"; // CompBackGround
        public static string Theme_CherryBlossomColor3 = "Theme_CherryBlossomColor3"; // UnSelectedColor
        public static string Theme_CherryBlossomColor4 = "Theme_CherryBlossomColor4"; // SelectedColor
        public static string Theme_CherryBlossomColor5 = "Theme_CherryBlossomColor5"; // FontColor


    }


    public class MIYO_Theme
    {
        public static List<SolidColorBrush> GetAllThemeBrush() 
        {
            List<SolidColorBrush> solidColorBrushes = new List<SolidColorBrush>();

            solidColorBrushes.Add(MIYO_Theme.GetThemeBrush(ThemeBrushName.Brush_MainBackGround));
            solidColorBrushes.Add(MIYO_Theme.GetThemeBrush(ThemeBrushName.Brush_CompBackGround));
            solidColorBrushes.Add(MIYO_Theme.GetThemeBrush(ThemeBrushName.Brush_UnSelectedColor));
            solidColorBrushes.Add(MIYO_Theme.GetThemeBrush(ThemeBrushName.Brush_SelectedColor));
            solidColorBrushes.Add(MIYO_Theme.GetThemeBrush(ThemeBrushName.Brush_FontColor1));
            solidColorBrushes.Add(MIYO_Theme.GetThemeBrush(ThemeBrushName.Brush_FontColor2));
            return solidColorBrushes;
        }


        public static SolidColorBrush GetThemeBrush(string ResourceName)
        {
            if (Application.Current.Resources[ResourceName] is SolidColorBrush brush)
            {
                return brush;
            }
            else 
            {
                throw new Exception("ResourceName not a SolidColorBrush");
            }
        }

        public static Color GetThemeColor(string ResourceName)
        {
            if (Application.Current.Resources[ResourceName] is Color color)
            {
                return color;
            }
            else
            {
                throw new Exception("ResourceName not a Color");
            }
        }


    }
}
