using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TranslationTwitter
{

    internal class MainView : ListView{
        public event EventHandler Refresh;

        public delegate void ToggleHandler(Item item);
        public event ToggleHandler Toggle;

        public MainView(ObservableCollection<Item> tweets, ListViewHeader listViewHeader)
        {
            ItemsSource = tweets; //データソースの指定
            ItemTemplate = new DataTemplate(() => new MyCell(Toggle));//セルの指定
            HasUnevenRows = true; //行の高さを可変とする
            BackgroundColor = Color.FromRgba(255,255,255,50);
            IsPullToRefreshEnabled = true;//Pull To Refresh有効
            Refreshing += (s, a) =>{
                if (Refresh != null) {
                    Refresh(s, a);
                    ScrollTo(tweets[0], ScrollToPosition.Start, false);//一番上まで表示
                }
                IsRefreshing = false;
            };
            //ヘッダ
            Header = listViewHeader;
            HeaderTemplate = new DataTemplate(typeof(HeaderTemlate));
            ItemSelected += (sender, args) => {
                var x = 0;
            };
        }
        
        //ヘッダのテンプレート
        private class HeaderTemlate :ContentView{
            public HeaderTemlate() {

                BackgroundColor = Color.Navy;

                var labelCount = new Label {
                    TextColor = Color.White,
                    Font = Font.SystemFontOfSize(12),
                };
                labelCount.SetBinding(Label.TextProperty, "Count");

                var labelLastUpdate = new Label {
                    TextColor = Color.White,
                    Font = Font.SystemFontOfSize(12),
                };
                labelLastUpdate.SetBinding(Label.TextProperty, "LastUpdate");


                Content = new StackLayout {
                    Padding = 5,
                    Orientation = StackOrientation.Horizontal,
                    Children = { labelCount,labelLastUpdate }
                };
            }
        }
      

        //セル用のテンプレート
        private class MyCell : ViewCell {
            
            readonly int _fontSizeText = Device.OnPlatform(12, 12, 18);
            readonly int _fontSizeName = Device.OnPlatform(10, 10, 16);
            readonly int _fontSizeScreenName = Device.OnPlatform(14, 14, 20);
            readonly int _fontSizeCreateAt = Device.OnPlatform(10, 10, 16);

            public MyCell(ToggleHandler Toggle){
                //アイコン
                var icon = new Image();
                icon.WidthRequest = icon.HeightRequest = 50; //アイコンのサイズ
                icon.VerticalOptions = LayoutOptions.Start; //アイコンを行の上に詰めて表示
                icon.SetBinding(Image.SourceProperty, "Icon");

                //名前
                var name = new Label {
                    Font = Font.SystemFontOfSize(_fontSizeName),
                    TextColor = Color.Black

                };
                name.SetBinding(Label.TextProperty, "Name");

                //アカウント名
                var screenName = new Label {
                    Font = Font.SystemFontOfSize(_fontSizeScreenName),
                    TextColor = Color.Black
                };
                screenName.SetBinding(Label.TextProperty, "ScreenName");

                //作成日時
                var createAt = new Label {
                    Font = Font.SystemFontOfSize(_fontSizeCreateAt), 
                    TextColor = Color.Gray,
                };
                createAt.SetBinding(Label.TextProperty, "CreatedAt");

                //メッセージ本文
                var text = new Label {
                    Font = Font.SystemFontOfSize(_fontSizeText),
                    TextColor = Color.Black
                };
                text.SetBinding(Label.TextProperty, "Text");


                ////ID(デバッグ用)
                //var id = new Label
                //{
                //    Font = Font.SystemFontOfSize(10),
                //    TextColor = Color.Red
                //};
                //id.SetBinding(Label.TextProperty, "Id");



                //名前行
                var layoutName = new StackLayout {
                    Orientation = StackOrientation.Horizontal, //横に並べる
                    Children = {name, screenName} //名前とアカウント名を横に並べる
                };

                //サブレイアウト
                var layoutSub = new StackLayout {
                    Spacing = 0, //スペースなし
                    //Children = {layoutName, createAt, text,id} //名前行、作成日時、メッセージを縦に並べる
                    Children = {layoutName, createAt, text} //名前行、作成日時、メッセージを縦に並べる
                };

                View = new StackLayout {
                    //Padding = new Thickness(5),
                    Orientation = StackOrientation.Horizontal, //横に並べる
                    //BackgroundColor = Color.FromRgba(255, 255, 255, 80),
                    Children = {icon, layoutSub} //アイコンとサブレイアウトを横に並べる
                };

                var menuItem = new MenuItem
                {
                    Text = "Convert",
                    Command = new Command(p => { //コンテキストメニュー
                        if (Toggle != null) {
                            Toggle((Item) p);
                        }
                    }),
                    IsDestructive = true, //背景赤色
                };

                menuItem.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));//コマンド実行時のパラメータに表示テキストを使用する
                ContextActions.Add(menuItem);

            }

            //テキストの長さに応じて行の高さを増やす
            protected override void OnBindingContextChanged() {
                base.OnBindingContextChanged();

                //メッセージ
                var text = ((Item) BindingContext).Text;
                //メッセージを改行で区切って、各行の最大文字数を27として行数を計算する（27文字は、日本を基準にしました）
                var len = 27;
                var row = text.Split('\n').Select(l => l.Length/len).Select(c => c + 1).Sum();
                Height = _fontSizeScreenName + _fontSizeCreateAt + row * _fontSizeText + 20; //名前行、作成日時行、メッセージ行、パディングの合計値
                if (Height < 60) {
                    Height = 60; //列の高さは、最低でも60とする
                }
                var isTranslated = ((Item)BindingContext).IsTranslated;
                if (isTranslated) {
                    View.BackgroundColor = Color.FromRgb(220, 220, 220);
                }
            }
        }
    }
}
