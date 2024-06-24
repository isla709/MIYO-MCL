using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIYO_Weather.Qweather.QweatherReceiveType
{
    public class QW_CityListData
    {
        public string code {  get; set; }
        public QW_CityInfo[] topCityList;

    }
}
