using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板
//apiKey: f81aedad04049be83d8625a378e7d201
namespace InternetAccess
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public object JsonConvert { get; private set; }

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void weatherButton_Click(object sender, RoutedEventArgs e)
        {
            Root weatherInfo = await Weather.GetWeather(City.Text);
            String info = "";
            if(weatherInfo.resultcode == "200")
            {
                info += "天气： " + weatherInfo.result.today.weather + "\n";
                info += "温度： " + weatherInfo.result.today.temperature + "\n";
                info += "风向： " + weatherInfo.result.today.wind + "\n";
                info += "建议穿衣： " + weatherInfo.result.today.dressing_advice + "\n";
            }
            else
            {
                info = "查询失败！\n";
            }
            weatherResult.Text = info;
        }

        private void idButton_Click(object sender, RoutedEventArgs e)
        {
            queryId(Id.Text);
        }

        private async void queryId(string str)
        {
            string url = string.Format("http://api.k780.com:88/?app=idcard.get&idcard={0}&appkey=33217&sign=30e31255c15c77e043c3d8d4f5345ffc&format=xml", str);
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync(url);

            XmlDocument document = new XmlDocument();
            document.LoadXml(result);
            String personInfo = "";

            //get the location
            XmlNodeList NodeList = document.GetElementsByTagName("att");
            IXmlNode node = NodeList.Item(0);
            if(node == null)
            {
                idResult.Text = "查询失败\n";
                return;
            }
            personInfo += "归属地： " + node.InnerText + "\n";

            //get the area code
            NodeList = document.GetElementsByTagName("areano");
            node = NodeList.Item(0);
            personInfo += "区号： " + node.InnerText + "\n";

            //get the post code
            NodeList = document.GetElementsByTagName("postno");
            node = NodeList.Item(0);
            personInfo += "邮编： " + node.InnerText + "\n";

            //get the gender
            NodeList = document.GetElementsByTagName("sex");
            node = NodeList.Item(0);
            personInfo += "性别： " + node.InnerText + "\n";

            idResult.Text = personInfo;
        }
    }
}
