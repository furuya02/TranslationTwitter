using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TranslationTwitter
{
    public class SearchBehavior : Behavior<Entry>
    {
        private readonly Regex _regex = new Regex(@"^[a-zA-Z]+$");
        
        //クラス外から検証結果が分かるようにプロパティを追加
        public bool IsValid { get; private set; }

        protected override void OnAttachedTo(Entry bindable){
            IsValid = _regex.Match(bindable.Text).Success;

            //ハンドラの追加
            bindable.TextChanged += CheckSearchStr;

        }

        protected override void OnDetachingFrom(Entry bindable){
            //ハンドラの削除
            bindable.TextChanged -= CheckSearchStr;
        }

        private void CheckSearchStr(object sender, TextChangedEventArgs e){
            var m = _regex.Match(e.NewTextValue);
            IsValid = m.Success;
            ((Entry)sender).TextColor = IsValid ? Color.Default : Color.Red;
        }
    }
}
