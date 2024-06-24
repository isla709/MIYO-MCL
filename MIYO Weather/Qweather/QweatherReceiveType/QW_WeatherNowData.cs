using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIYO_Weather.Qweather.QweatherReceiveType
{
    public class QW_WeatherNowData
    {
        public string code {  get; set; }
        public string updateTime { get; set; }
        public string fxLink {  get; set; }
        public QW_WeatherNowInfo now {  get; set; }
    }
}
