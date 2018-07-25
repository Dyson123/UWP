using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Xml.Serialization;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;
using System.ComponentModel;

namespace MyList.Models
{
 
    public class TodoItem:INotifyPropertyChanged
    {

        private string id;

        private string _title;
        private bool _completed;
        private ImageSource _imagerUrl;

        public string getId()
        {
            return id;
        }

        public string title { 
            get { return _title; }
            set {
                _title = value;
                RaisePropertyChanged("title");
        } }

        public string description
        {
            get;
            set;
        }

        public Boolean completed { 
            get { return _completed; ; }
            set
            {
                _completed = value;
                RaisePropertyChanged("completed");
            }
        }

        public DateTime date
        {
            get;
            set;
        }

        public ImageSource imagerUrl { 
            get { return _imagerUrl; }
            set {
                _imagerUrl = value;
                RaisePropertyChanged("imagerUrl");
        } }

        public TodoItem(string title, string description, DateTime date, ImageSource defaultUrl)
        {
            this.id = Guid.NewGuid().ToString();//生成id
            this.title = title;
            this.description = description;
            this.completed = false;//默认为未完成
            this.date = date;
            this.imagerUrl = defaultUrl;
        }
        public TodoItem(string id,string title, string description, DateTime date, String com,ImageSource defaultUrl)
        {
            this.id = id;
            this.title = title;
            this.description = description;
            if (com == "false")
                this.completed = false;
            else
                this.completed = true;
            this.date = date;
            this.imagerUrl = defaultUrl;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
