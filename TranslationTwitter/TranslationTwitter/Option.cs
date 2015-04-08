using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TranslationTwitter
{
    public class Option {

        private readonly ListView _listView;

        private bool _separator;
        public bool Separator {
            get { return _separator;}
            set {
                _separator = value;
                _listView.SeparatorVisibility = _separator ? SeparatorVisibility.Default : SeparatorVisibility.None;
            }
        }

        public int ListViewMax { get; set; }

        public Option(ListView listView) {
            _listView = listView;
            ListViewMax = 100;
        }
    
    }
}
