using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.IO;
using MIYO_Weather.Qweather.QweatherReceiveType;
using Newtonsoft.Json;

namespace MIYO_Weather.Qweather
{
    /// <summary>
    /// 和风天气API
    /// </summary>
    public class QweatherAPI
    {
        /// <summary>
        /// 用户APIKEY
        /// </summary>
        public string Apikey { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public QweatherAPI() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="apikey">用户APIKEY</param>
        public QweatherAPI(string apikey)
        {
            Apikey = apikey;
        }

        /// <summary>
        /// 异步获取城市列表
        /// </summary>
        /// <returns>经过反序列化的Json数据</returns>
        public async Task<QW_CityListData?> GetCityListAsync()
        {
            try
            {
                string recvjson = await MIYO_GzipTool.DezipToString(await MIYO_HTTPRequest.GetAsync<Stream>(QweatherURLBuilder.CityListURLBuilder(QweatherURL.CityListURL, Apikey, number: 20)), Encoding.UTF8);
                return JsonConvert.DeserializeObject<QW_CityListData>(recvjson);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Message:" + ex.Message + "  Source:" + ex.Source, "Error");
                return null;
            }
            
        }

        /// <summary>
        /// 异步查找城市
        /// </summary>
        /// <param name="admName">省名</param>
        /// <param name="cityName">城市名</param>
        /// <returns>经过反序列化的Json数据</returns>
        public async Task<QW_CityFindData?> GetCityFindAsync(string admName, string cityName, string _lang = "zh")
        {
            try 
            {
                string recvjson = await MIYO_GzipTool.DezipToString(await MIYO_HTTPRequest.GetAsync<Stream>(QweatherURLBuilder.CityFindURLBuilder(QweatherURL.CityFindURL, Apikey, admName, cityName, number: 20, lang: _lang)), Encoding.UTF8);
                return JsonConvert.DeserializeObject<QW_CityFindData>(recvjson);
            }
            catch (Exception ex) 
            {
                Trace.WriteLine("Message:" + ex.Message + "  Source:" + ex.Source, "Error");
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityname">城市ID/城市名</param>
        /// <param name="_lang"></param>
        /// <returns></returns>
        public async Task<QW_CityFindData?> GetCityFindAsync(string cityname, string _lang = "zh")
        {
            try
            {
                string recvjson = await MIYO_GzipTool.DezipToString(await MIYO_HTTPRequest.GetAsync<Stream>(QweatherURLBuilder.CityFindURLBuilder(QweatherURL.CityFindURL, Apikey, cityname, number: 20, lang: _lang)), Encoding.UTF8);
                return JsonConvert.DeserializeObject<QW_CityFindData>(recvjson);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Message:" + ex.Message + "  Source:" + ex.Source, "Error");
                return null;
            }
        }

        /// <summary>
        /// 异步获取当前天气
        /// </summary>
        /// <param name="cityid">城市ID</param>
        /// <param name="isFree">是否为免费订阅</param>
        /// <returns>经过反序列化的Json数据</returns>
        public async Task<QW_WeatherNowData?> GetWeatherNowAsync(string cityid, bool isFree = true, string _lang = "zh")
        {
            try 
            {
                if (isFree)
                {
                    string recvjson = await MIYO_GzipTool.DezipToString(await MIYO_HTTPRequest.GetAsync<Stream>(QweatherURLBuilder.WeatherURLBuilder(QweatherURL.WeatherNowURL_Free, Apikey, cityid, _lang)), Encoding.UTF8);
                    return JsonConvert.DeserializeObject<QW_WeatherNowData>(recvjson);
                }
                else
                {
                    string recvjson = await MIYO_GzipTool.DezipToString(await MIYO_HTTPRequest.GetAsync<Stream>(QweatherURLBuilder.WeatherURLBuilder(QweatherURL.WeatherNowURL, Apikey, cityid, _lang)), Encoding.UTF8);
                    return JsonConvert.DeserializeObject<QW_WeatherNowData>(recvjson);
                }
            }
            catch (Exception ex) 
            {
                Trace.WriteLine("Message:" + ex.Message + "  Source:" + ex.Source, "Error");
                return null;
            }
        }

        /// <summary>
        /// 异步获取未来N天天气
        /// </summary>
        /// <param name="cityid">城市ID</param>
        /// <param name="day">天数(范围为3，7)</param>
        /// <param name="isFree">是否为免费订阅</param>
        /// <returns>经过反序列化的Json数据</returns>
        public async Task<QW_WeatherDayData?> GetWeatherDayAsync(string cityid, int day = 3, bool isFree = true, string _lang = "zh")
        {
            try
            {
                if (isFree)
                {
                    switch (day)
                    {
                        case 3:
                            string recvjson3 = await MIYO_GzipTool.DezipToString(await MIYO_HTTPRequest.GetAsync<Stream>(QweatherURLBuilder.WeatherURLBuilder(QweatherURL.Weather3DayURL_Free, Apikey, cityid, _lang)), Encoding.UTF8);
                            return JsonConvert.DeserializeObject<QW_WeatherDayData>(recvjson3);
                        case 7:
                            string recvjson7 = await MIYO_GzipTool.DezipToString(await MIYO_HTTPRequest.GetAsync<Stream>(QweatherURLBuilder.WeatherURLBuilder(QweatherURL.Weather7DayURL_Free, Apikey, cityid , _lang)), Encoding.UTF8);
                            return JsonConvert.DeserializeObject<QW_WeatherDayData>(recvjson7);
                        default:
                            throw new Exception("参数Day的取值无效");
                    }
                }
                else
                {
                    switch (day)
                    {
                        case 3:
                            string recvjson3 = await MIYO_GzipTool.DezipToString(await MIYO_HTTPRequest.GetAsync<Stream>(QweatherURLBuilder.WeatherURLBuilder(QweatherURL.Weather3DayURL, Apikey, cityid, _lang)), Encoding.UTF8);
                            return JsonConvert.DeserializeObject<QW_WeatherDayData>(recvjson3);
                        case 7:
                            string recvjson7 = await MIYO_GzipTool.DezipToString(await MIYO_HTTPRequest.GetAsync<Stream>(QweatherURLBuilder.WeatherURLBuilder(QweatherURL.Weather7DayURL, Apikey, cityid, _lang)), Encoding.UTF8);
                            return JsonConvert.DeserializeObject<QW_WeatherDayData>(recvjson7);
                        default:
                            throw new Exception("参数Day的取值无效");
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Message:" + ex.Message + "  Source:" + ex.Source, "Error");
                return null;
            }

        }

    }
}
