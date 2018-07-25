using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace InternetAccess
{
    public class Weather
    {
        public async static Task<Root> GetWeather(String str)
        {
            var http = new HttpClient();
            var response = await http.GetAsync(String.Format("http://v.juhe.cn/weather/index?format=1&cityname={0}&key=f81aedad04049be83d8625a378e7d201", str));
            var result = await response.Content.ReadAsStringAsync();

            // 序列化
            var serializer = new DataContractJsonSerializer(typeof(Root)); 

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (Root)serializer.ReadObject(ms);
            return data;
        }
    }

    [DataContract]
    public class Today
    {
        [DataMember]
        public string temperature { get; set; }
        
        [DataMember]
        public string weather { get; set; }
        
        [DataMember]
        public string wind { get; set; }

        [DataMember]
        public string dressing_advice { get; set; }
    }
    
    [DataContract]
    public class Result
    {
        [DataMember]
        public Today today { get; set; }
    }

    [DataContract]
    public class Root
    {
        [DataMember]
        public string resultcode { get; set; }

        [DataMember]
        public Result result { get; set; }
    }
}
