using Xamarin.Forms;

namespace TranslationTwitter {
    //検索文字列入力時に上部に表示されるバー
    public class SearchView : ContentView {
        private readonly Entry _entry;

        public delegate void OnEnterHandler(string str);
        public event OnEnterHandler OnEnter;

        public SearchView() {

            HeightRequest = 0; //デフォルトで非表示

            var buttonStyle = new Style(typeof (Button)) {
                Setters = {
                    new Setter {Property = Button.BackgroundColorProperty, Value = Color.Transparent},
                    new Setter {Property = Button.BorderColorProperty, Value = Color.White},
                    new Setter {Property = Button.BorderWidthProperty, Value = 1},
                    new Setter {Property = Button.BorderRadiusProperty, Value = 5},
                    new Setter{Property = Button.TextColorProperty,Value = Color.White},
                    new Setter{Property = Button.HorizontalOptionsProperty,Value = LayoutOptions.End},
                }
            };


            //ビヘイビア
            var searchBehavior = new SearchBehavior();

            //「OK」ボタン
            var okButton = new Button{
                Text = "ＯＫ",
                Style = buttonStyle,
                Command = new Command(StartSearch)//検索開始
            };

            //「キャンセル」ボタン
            var calcelButton = new Button{
                Text = "キャンセル",
                Style = buttonStyle,
                Command = new Command(Close)
            };

            //検索文字入力欄
            _entry = new Entry
            {
                Placeholder = "ここに検索文字を入力してください",
                Text = "",
                //HorizontalOptions = LayoutOptions.FillAndExpand,//他のコントロールの動的な変化に、うまく動作できない
                WidthRequest = Device.OnPlatform(250,160,200),
                Behaviors = { searchBehavior }, //ビヘイビアの追加
            };
            _entry.Completed += (s, a) => StartSearch();//検索開始
            _entry.TextChanged += (s, a) =>{
                okButton.IsVisible = searchBehavior.IsValid;
            };
           

            Content = new StackLayout {
                Padding = new Thickness(10, 0, 10, 0),
                Spacing = 5,
                Orientation = StackOrientation.Horizontal,
                Children = { _entry, okButton,calcelButton }
            };

        }

        //検索開始
        void StartSearch() {
            if (OnEnter != null){
                OnEnter(_entry.Text);
            }
            Close();
        }

        //この検索ビューの表示
        public async void Open(string searchStr) {
            _entry.Text = searchStr;

            HeightRequest = Device.OnPlatform(50,50,70); //表示(WindowsPhoneの場合のみ70)
            await this.ScaleTo(1, 550, Easing.SinIn); //アニメーション
            _entry.Focus();//キーボード表示

        }

        //この検索ビューの非表示
        public async void Close()
        {
            await this.ScaleTo(0, 550, Easing.SinIn); //アニメーション
            HeightRequest = 0; //非表示
            _entry.Unfocus();//キーボード非表示
        }
    }
}
