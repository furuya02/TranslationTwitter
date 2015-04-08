using System;
using Xamarin.Forms;

namespace TranslationTwitter {
    //CommandBarの中で使用されるコマンドボタン
    internal class CommandButton : AbsoluteLayout {

        public event EventHandler Clicked;

        public CommandButton(ImageSource imageSource, string title) {

            var image = new Image {
                Source = imageSource
            };
            var label = new Label {
                Text = title,
                FontSize = Device.OS == TargetPlatform.WinPhone ? 20 : 10,
                TextColor = Color.White,
            };
            var button = new Button {
                WidthRequest = Device.OS == TargetPlatform.WinPhone ? 65 : 35,
                HeightRequest = Device.OS == TargetPlatform.WinPhone ? 65 : 35,
                BackgroundColor = Color.Transparent,
                BorderColor = Color.Transparent, //WindowsPhoneで必要
                
            };
            button.Clicked += (s, a) => {
                if (Clicked != null) {
                    Clicked(s, a);
                }
            };

            Children.Add(image, new Point(5, 0));

            Children.Add(label, Device.OS == TargetPlatform.WinPhone ? new Point(0, 45) : new Point(0, 25));
            Children.Add(button, new Point(0, 0));

            HorizontalOptions = LayoutOptions.End;
        }
    }
}