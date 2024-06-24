using MIYO_Weather.Qweather;
using MIYO_Weather.Qweather.QweatherReceiveType;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Windows.ApplicationModel.Store.Preview.InstallControl;


namespace MIYO_MCL.Class
{
    public struct Iconlist
    { 
        public IconlistData[] data;
    }

    public struct IconlistData 
    {
        public string icon_code;
        public string icon_name;
    }


    public class MIYO_SyncWeatherToControl
    {

        public TextBlock? WeatherStatus;
        public TextBlock? WeatherTemp;
        public TextBlock? WeatherIcon;
        public TextBlock? WeatherCity;

        private QweatherAPI qweatherAPI;
        private MIYO_ConfigManager configManager;
        private AppConfigData configData;
        

        public MIYO_SyncWeatherToControl(MIYO_ConfigManager _ConfigManager)
        {
            configManager = _ConfigManager;

            qweatherAPI = new QweatherAPI();

            try
            {
                configData = configManager.DeserializationAppConifgJson(configManager.ReadConfigFile());
                qweatherAPI.Apikey = configData.QWeatherKey;

            }
            catch (Exception e)
            {
                Trace.WriteLine(e, "Error");
            }
        }

        public Iconlist ReadIconMap()
        {
            string iconmapPath = "MIYO_MCL.Asset.Json.icons-list.json";
            using Stream? iconmapStream = MIYO_EmbeddedResource.LoadAssetStream(iconmapPath);
            if (iconmapStream == null) { throw new Exception("内部资源读取失败"); }
            using StreamReader streamReader = new StreamReader(iconmapStream);
            return JsonConvert.DeserializeObject<Iconlist>(streamReader.ReadToEnd());
        }

        public async void SyncToControl(MainWindow mainWindow)
        {
            try
            {
                configData = configManager.DeserializationAppConifgJson(configManager.ReadConfigFile());

                await Task.Run(() => 
                {
                    if (configData.CityID == null)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            configData = configManager.DeserializationAppConifgJson(configManager.ReadConfigFile());
                            if (configData.CityID != null)
                            {
                                break;
                            }
                            Trace.WriteLine("AppConfig出现错误，正在尝试重新初始化，第" + (i + 1).ToString() + "次尝试");
                        }
                    }
                });

                QW_WeatherNowData? weatherNowData = await qweatherAPI.GetWeatherNowAsync(configData.CityID,_lang:"en"); //获取天气默认使用免费模式，需要使用付费定义请设置isFree为False
                if (weatherNowData == null)
                {
                    throw new Exception("向QweatherAPI获取数据失败");
                }

                mainWindow.weatherNowData = weatherNowData;
                if (WeatherStatus != null)
                {
                    WeatherStatus.Text = weatherNowData.now.text;
                }
                if (WeatherTemp != null)
                {
                    WeatherTemp.Text = weatherNowData.now.temp + "°";
                }
                if (WeatherIcon != null)
                {
                    var iconmap = ReadIconMap();
                    for (int i = 0; i < iconmap.data.Length; i++)
                    {
                        if (weatherNowData.now.icon == iconmap.data[i].icon_code)
                        {
                            string icon = char.ConvertFromUtf32(0xf1 * 256 + (i + 1));
                            WeatherIcon.Text = icon;
                        }
                    }
                }
                if (WeatherCity != null)
                {
                    var citydata = await qweatherAPI.GetCityFindAsync(configData.CityID);
                    if (citydata == null) { throw new Exception("API Error Check Network And API Key"); }
                    mainWindow.cityFindData = citydata;
                    WeatherCity.Text = citydata.location[0].adm1 + " " + citydata.location[0].name;
                }
                mainWindow.comp_Weather.Visibility = System.Windows.Visibility.Visible;

            }
            catch (Exception e) 
            {
                mainWindow.comp_Weather.Visibility = System.Windows.Visibility.Hidden;
                if (qweatherAPI.Apikey == "" || string.IsNullOrEmpty(qweatherAPI.Apikey)) 
                {
                    Trace.WriteLine("天气API未配置，无法使用天气组件。你可以在AppConfig.json中配置QWeatherKey", "Notice");
                    Trace.WriteLine("你可以配置使用自己的和风天气API以启用天气组件","Notice");
                    Trace.WriteLine("源码版不包含作者提供的KEY,请在项目中自行按需求设置", "Notice");
                }
                

                Trace.WriteLine(e, "Error");
                Trace.WriteLine("天气API无法获取数据，检查网络和API", "Error");
            }
            
        
        }

        public QweatherAPI GetQweatherAPI()
        {
            return qweatherAPI; 
        }



    }
}
