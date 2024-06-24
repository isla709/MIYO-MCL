using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIYO_Weather.Qweather.QweatherReceiveType
{
    public class QW_WeatherDayData
    {
        public string code;
        public string updateTime;
        public string fxLink;
        public QW_WeatherDayInfo[] daily;
    }
}
