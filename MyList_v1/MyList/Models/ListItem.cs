using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;


namespace MyList.Models
{
    public class TodoItem
    {
        private string id;

        public string title { get; set; }

        public string description {get; set;}
        
        public bool completed { get; set; }

        public DateTime date { get; set; }

        public ImageSource imagerUrl { get; set; }

        public TodoItem(string title, string description, DateTime date,ImageSource defaultUrl)
        {
            this.id = Guid.NewGuid().ToString();//生成id
            this.title = title;
            this.description = description;
            this.completed = false;//默认为未完成
            this.date = date;
            this.imagerUrl = defaultUrl;
        }

 
    }
}
