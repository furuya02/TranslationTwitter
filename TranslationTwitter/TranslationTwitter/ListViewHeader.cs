using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TranslationTwitter{

    class ListViewHeader:BindableObject{


        public static readonly BindableProperty CountProperty = BindableProperty.Create<ListViewHeader, String>(p => p.Count, "");
        public String Count{
            get { return (String)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }

        public static readonly BindableProperty LastUpdateProperty = BindableProperty.Create<ListViewHeader, String>(p => p.LastUpdate, "");
        public String LastUpdate{
            get { return (String)GetValue(LastUpdateProperty); }
            set { SetValue(LastUpdateProperty, value); }
        }
    }
}
