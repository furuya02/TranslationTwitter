
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TranslationTwitter {

    //１つのTweetを表現するクラス
    internal class Item{
        public string Name { get; set; } //表示名
        public string Text { get; set; } //メッセージ
        public string ScreenName { get; set; } //アカウント名
        public string CreatedAt { get; set; } //作成日時
        public string Icon { get; set; } //アイコン
        public long Id { get; set; } //Id
        public string TranslatedText { get; set; } //翻訳済みテキスト
        public bool IsTranslated { get; set; }
    }

    //Textプロパティの変更で再表示するには、INotifyPropertyChangedの実装が必要になる
    //internal class Item : INotifyPropertyChanged    {
    //    public string Name { get; set; } //表示名
    //    //public string Text { get; set; } //メッセージ
    //    public string ScreenName { get; set; } //アカウント名
    //    public string CreatedAt { get; set; } //作成日時
    //    public string Icon { get; set; } //アイコン
    //    public long Id { get; set; } //Id
    //    public string TranslatedText { get; set; } //翻訳済みテキスト
    //    public bool IsTranslated { get; set; }

    //    //メッセージ
    //    private string _text;
    //    public string Text
    //    {
    //        get { return _text; }
    //        set { SetField(ref _text, value, "Text"); }
    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;
    //    protected virtual void OnPropertyChanged(string propertyName)
    //    {
    //        PropertyChangedEventHandler handler = PropertyChanged;
    //        if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    //    }
    //    protected bool SetField<T>(ref T field, T value, string propertyName)
    //    {
    //        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
    //        field = value;
    //        OnPropertyChanged(propertyName);
    //        return true;
    //    }
    //}
}

