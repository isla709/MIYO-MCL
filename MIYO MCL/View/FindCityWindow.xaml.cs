using Panuon.WPF.UI;
using MIYO_Weather.Qweather;
using MIYO_Weather.Qweather.QweatherReceiveType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;

namespace MIYO_MCL.View
{
    public delegate void OnFindCityWindowFinish(QW_CityInfo cityData);


    /// <summary>
    /// FindCityWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FindCityWindow : WindowX
    {
        public event OnFindCityWindowFinish OnFindCityWindowFinish;

        private QweatherAPI qweatherAPI;

        private List<QW_CityInfo> cityInfos;

        public FindCityWindow(QweatherAPI API)
        {
            InitializeComponent();

            qweatherAPI = API;

        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void btn_runfind_Click(object sender, RoutedEventArgs e)
        {

            

            try 
            {
                lb_results.Items.Clear();
                var cityFindData = await qweatherAPI.GetCityFindAsync(tb_cityName.Text);
                cityInfos = cityFindData.location.ToList();
                cityInfos.ForEach(cityInfo => lb_results.Items.Add(string.Format("{0} {1}   ID:{2}", cityInfo.adm1, cityInfo.name, cityInfo.id)));
            }
            catch (Exception ex) 
            {
                Trace.WriteLine(ex);
            
            }
           


        }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                if(lb_results.SelectedIndex == -1)
                {
                    Toast("请先选择一个城市");
                    return;
                }
                OnFindCityWindowFinish.Invoke(cityInfos[lb_results.SelectedIndex]);
            } 
            catch (Exception ex)
            {
                Trace.WriteLine(ex); 
            };
           
            
        }
    }
}
