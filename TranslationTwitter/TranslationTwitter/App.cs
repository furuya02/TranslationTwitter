using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TranslationTwitter
{
    public class App : Application {

        public App()
        {
            MainPage = new MyPage();
        }

        protected override void OnStart() {
            ((MyPage)MainPage).Read();
        }

        protected override void OnSleep() {
            ((MyPage)MainPage).Save();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }

    internal class MyPage : ContentPage {
        
        private readonly Translator _translator = new Translator();
        //データソース（class Tweetのコレクション）
        private readonly ObservableCollection<Item> _items = new ObservableCollection<Item>();
        //リストビューのヘッダ情報
        private readonly ListViewHeader _listViewHeader = new ListViewHeader {
            Count = "0件",
            LastUpdate = "----/--/--"
        };
        //オプション
        private Option _option;
        readonly Twitter _twitter = new Twitter();
        private string _searchStr="Xamarin";

        public MyPage()
        {
            BackgroundColor = Color.FromRgb(56, 190, 255);

            //メインビュー（Twitter表示領域）
            var mainView = new MainView(_items, _listViewHeader);
            mainView.Refresh += (sender, args) =>
            {
                Refresh(_searchStr);
            };
            mainView.Toggle += Toggle;

            _option = new Option(mainView);

            //オプションビュー
            var optionView = new OptionView(_option);

            //検索ビュー
            var searchView = new SearchView();
            searchView.OnEnter += str => {
                _searchStr = str;
                _items.Clear();
                Refresh(_searchStr);
            };
            //コマンドビュー
            var commandView = new CommandView();
            commandView.OnSeach += (s, a) =>{
                optionView.Close();
                searchView.Open(_searchStr);//検索バーの表示
            };
            commandView.OnOption += (s, a) => {
                searchView.Close();
                optionView.Open();
            };

            Content = new StackLayout{
                Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0),
                Children = { optionView,searchView, mainView, commandView }
            };
        }



        async void Toggle(Item item) {

            //未翻訳の場合
            if (item.TranslatedText == null){
                item.TranslatedText = await _translator.Get(item.Text);

            }
            var tmpStr = item.Text;
            item.Text = item.TranslatedText;
            item.TranslatedText = tmpStr;

            item.IsTranslated = !item.IsTranslated;

            //いったん削除して、新たに追加する(アニメーションのため)
            var index = _items.IndexOf(item);
            _items.RemoveAt(index);
            _items.Insert(index, item);

        }

        async void Refresh(string keyword) {
            var result = await _twitter.Search(_searchStr);

            foreach (var tweet in result.Reverse()) {

                if (!_items.Any(item => item.Id == tweet.Id)) {
                    _items.Insert(0,new Item
                    {
                        Text = tweet.Text,
                        Name = tweet.User.Name,
                        ScreenName = tweet.User.ScreenName,
                        CreatedAt = tweet.CreatedAt.ToString("f"),
                        Icon = tweet.User.ProfileImageUrl.ToString(),
                        Id = tweet.Id,
                        TranslatedText = null,//未翻訳の番兵
                        IsTranslated = false,
                    });
                }
            }

            while (_items.Count > _option.ListViewMax) {
                _items.RemoveAt(_items.Count-1);
            }

            _listViewHeader.Count = string.Format("{0}件", _items.Count);
            var dt = DateTime.Now;
            _listViewHeader.LastUpdate = String.Format("{0}/{1}/{2} {3}:{4} 更新", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute);
        }

        public void Save() {
            var p = Application.Current.Properties;
            
            //いったん全ての保存データを削除する
            try {
                p.Clear();
            }catch (Exception) {
                ;
            }

            p["ListViewMax"] = _option.ListViewMax;
            p["Separator"] = _option.Separator;
            p["Count"] = _listViewHeader.Count;
            p["LastUpdate"] = _listViewHeader.LastUpdate;
            p["searchStr"] = _searchStr;
            for (var i = 0; i < _items.Count; i++) {
                p["items_" + i] = _items[i];
            }
        }
        public void Read() {
            var p = Application.Current.Properties;

            var i = 0;
            while (true){
                var key = "items_" + i++;
                if (!p.ContainsKey(key)) {
                    break;
                }
                _items.Add(p[key] as Item);

            }

            if (p.ContainsKey("Count")) {
                _listViewHeader.Count = p["Count"] as String;
            }
            if (p.ContainsKey("LastUpdate")){
                _listViewHeader.LastUpdate = p["LastUpdate"] as String;
            }
            if (p.ContainsKey("ListViewMax")){
                _option.ListViewMax = p["ListViewMax"] as int? ?? 100;
            }
            if (p.ContainsKey("Separator")){
                _option.Separator = p["Separator"] as bool? ?? true;
            }
            if (p.ContainsKey("searchStr")){
                _searchStr = p["searchStr"] as string;
            }

        }
    }
}
