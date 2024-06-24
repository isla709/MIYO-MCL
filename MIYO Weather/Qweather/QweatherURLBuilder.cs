using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIYO_Weather.Qweather
{
    /// <summary>
    /// URL构建器类
    /// </summary>
    public static class QweatherURLBuilder
    {
        /// <summary>
        /// 城市列表URL构建器
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <param name="range"></param>
        /// <param name="number"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static string CityListURLBuilder(string url, string key, string range = "cn", int number = 10, string lang = "zh") 
        {
            return string.Format("{0}key={1}&range={2}&number={3}&lang={4}",url,key,range,number,lang);
        }


        /// <summary>
        /// 城市搜索URL构建器
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <param name="location"></param>
        /// <param name="range"></param>
        /// <param name="number"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static string CityFindURLBuilder(string url, string key, string location, string range = "cn", int number = 10, string lang = "zh")
        {
            return string.Format("{0}key={1}&location={2}&range={3}&number={4}&lang={5}", url, key, location, range, number, lang);
        }

        /// <summary>
        /// 城市搜索URL构建器
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <param name="adm"></param>
        /// <param name="location"></param>
        /// <param name="range"></param>
        /// <param name="number"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static string CityFindURLBuilder(string url, string key, string adm, string location, string range = "cn", int number = 10, string lang = "zh")
        {
            return string.Format("{0}key={1}&adm={2}&location={3}&range={4}&number={5}&lang={6}", url, key, adm, location, range, number, lang);
        }

        /// <summary>
        /// 实时天气/每日天气URL构建器
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <param name="location"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static string WeatherURLBuilder(string url, string key, string location, string lang = "zh") 
        {
            return string.Format("{0}key={1}&location={2}&lang={3}", url, key, location, lang);
        }

    }
}