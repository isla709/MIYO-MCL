using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIYO_Weather.Qweather
{
    /// <summary>
    /// 和风天气URL
    /// </summary>
    public static class QweatherURL
    {
        /// <summary>
        /// 获取城市列表的URL
        /// </summary>
        public readonly static string CityListURL = "https://geoapi.qweather.com/v2/city/top?";

        /// <summary>
        /// 搜索城市的URL
        /// </summary>
        public readonly static string CityFindURL = "https://geoapi.qweather.com/v2/city/lookup?";

        /// <summary>
        /// 实时天气URL
        /// </summary>
        public readonly static string WeatherNowURL = "https://api.qweather.com/v7/weather/now?";

        /// <summary>
        /// 实时天气URL(免费订阅)
        /// </summary>
        public readonly static string WeatherNowURL_Free = "https://devapi.qweather.com/v7/weather/now?";

        /// <summary>
        /// 未来3天天气URL
        /// </summary>
        public readonly static string Weather3DayURL = "https://api.qweather.com/v7/weather/3d?";

        /// <summary>
        /// 未来3天天气URL(免费订阅)
        /// </summary>
        public readonly static string Weather3DayURL_Free = "https://devapi.qweather.com/v7/weather/3d?";

        /// <summary>
        /// 未来7天天气URL
        /// </summary>
        public readonly static string Weather7DayURL = "https://api.qweather.com/v7/weather/7d?";

        /// <summary>
        /// 未来7天天气URL(免费订阅)
        /// </summary>
        public readonly static string Weather7DayURL_Free = "https://devapi.qweather.com/v7/weather/7d?";

    }
}
