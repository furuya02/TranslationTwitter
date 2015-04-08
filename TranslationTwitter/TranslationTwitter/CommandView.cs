using System;
using Xamarin.Forms;

namespace TranslationTwitter {

    //画面最下部に表示されるコマンドバー
    public class CommandView : ContentView {

        public event EventHandler OnSeach;
        public event EventHandler OnOption;

        public CommandView() {

            //設定ボタン
            var optionButton = new CommandButton(ImageSource.FromResource("TranslationTwitter.Images.option.png"), "Option");
            optionButton.Clicked += (s, a) =>
            {
                if (OnOption != null)
                {
                    OnOption(s, a);
                }
            };
            //検索ボタン
            var searchButton = new CommandButton(ImageSource.FromResource("TranslationTwitter.Images.search.png"), "Search");
            searchButton.Clicked += (s, a) => {
                if (OnSeach != null) {
                    OnSeach(s, a);
                }
            };


            Content = new StackLayout {
                HorizontalOptions = LayoutOptions.End,

                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(10, 0, 10, 0),
                Children = { searchButton,optionButton }
            };
        }
    }
}