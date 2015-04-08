using System;
using Xamarin.Forms;

namespace TranslationTwitter
{
    public partial class OptionView : ContentView {

        private readonly Option _option;

        public OptionView(Option option){
            InitializeComponent();

            _option = option;
        }

        //ＯＫ
        void OnOk(object sender, EventArgs args) {
            //コントロールの状態をオプションに記録
            _option.Separator = SwitchsListViewSparator.IsToggled;
            _option.ListViewMax = Int32.Parse(EntryListViewMax.Text);
            Close();
        }
        //キャンセル
        void OnCancel(object sender, EventArgs args)
        {
            Close();
        }

        //このオプションビューの表示
        public void Open() {
            HeightRequest = 150;
            this.ScaleTo(1, 550, Easing.Linear); //アニメーション

            //オプション内容でコントロールを初期化
            SwitchsListViewSparator.IsToggled = _option.Separator;
            EntryListViewMax.Text = _option.ListViewMax.ToString();
        }

        //このオプションビューの非表示
        public void Close(){
            this.ScaleTo(0, 550, Easing.SinIn); //アニメーション
            HeightRequest = 0; //非表示
            EntryListViewMax.Unfocus();//キーボード非表示
        }

    }
    public class EntryValidation : TriggerAction<Entry>{
        protected override void Invoke(Entry sender) {
            int n;
            sender.TextColor = !Int32.TryParse(sender.Text, out n) ? Color.Red : Color.Blue;
        }
    }

}
