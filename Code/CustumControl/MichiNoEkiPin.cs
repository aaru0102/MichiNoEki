using Microsoft.Maui.Controls.Maps;

namespace RoadsideStationApp
{
    public class MichiNoEkiPin : Pin
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        public AutoNotifyProperty<string> Name { get; set; } = new AutoNotifyProperty<string>(string.Empty);

        /// <summary>
        /// 緯度
        /// </summary>
        public AutoNotifyProperty<double> Latitude { get; set; } = new AutoNotifyProperty<double>();

        /// <summary>
        /// 経度
        /// </summary>
        public AutoNotifyProperty<double> Longitude { get; set; } = new AutoNotifyProperty<double>();

        /// <summary>
        /// ピンの色
        /// </summary>
        public AutoNotifyProperty<Color> PinColor { get; set; } = new AutoNotifyProperty<Color>(Colors.DeepSkyBlue);

        /// <summary>
        /// 訪問状態
        /// </summary>
        public AutoNotifyProperty<bool> IsVisited { get; set; } = new AutoNotifyProperty<bool>();

        /// <summary>
        /// 注意事項あり
        /// </summary>
        public AutoNotifyProperty<bool> Notice { get; set; } = new AutoNotifyProperty<bool>();

        /// <summary>
        /// ピンの表示有無
        /// </summary>
        public AutoNotifyProperty<bool> Visibility { get; set; } = new AutoNotifyProperty<bool>(true);

        /// <summary>
        /// ID変更イベント
        /// </summary>
        public EventHandler<PinChangedEventArgs> NameChangedEvent { get; set; }

        /// <summary>
        /// 場所変更イベント
        /// </summary>
        public EventHandler<PinChangedEventArgs> LocationChangedEvent { get; set; }

        /// <summary>
        /// ピンの色変更イベント
        /// </summary>
        public EventHandler<PinChangedEventArgs> PinColorChangedEvent { get; set; }

        /// <summary>
        /// 訪問状態変更イベント
        /// </summary>
        public EventHandler<PinChangedEventArgs> IsVisitedChangedEvent { get; set; }

        /// <summary>
        /// 注意事項有無変更イベント
        /// </summary>
        public EventHandler<PinChangedEventArgs> NoticeChangedEvent { get; set; }

        /// <summary>
        /// ピンの表示有無変更イベント
        /// </summary>
        public EventHandler<PinChangedEventArgs> VisibilityChangedEvent { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
#pragma warning disable CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。'required' 修飾子を追加するか、Null 許容として宣言することを検討してください。
        public MichiNoEkiPin(MichiNoEkiInfo info)
#pragma warning restore CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。'required' 修飾子を追加するか、Null 許容として宣言することを検討してください。
        {
            // 各プロパティのPropertyChangedを捕捉してピンとしてのステータス変更イベントを発火する
            Name.PropertyChanged += (sender, e) => { NameChangedEvent?.Invoke(this, new PinChangedEventArgs(this)); };
            Latitude.PropertyChanged += (sender, e) => { LocationChangedEvent?.Invoke(this, new PinChangedEventArgs(this)); };
            Longitude.PropertyChanged += (sender, e) => { LocationChangedEvent?.Invoke(this, new PinChangedEventArgs(this)); };
            PinColor.PropertyChanged += (sender, e) => { PinColorChangedEvent?.Invoke(this, new PinChangedEventArgs(this)); };
            IsVisited.PropertyChanged += (sender, e) => { IsVisitedChangedEvent?.Invoke(this, new PinChangedEventArgs(this)); };
            Notice.PropertyChanged += (sender, e) => { NoticeChangedEvent?.Invoke(this, new PinChangedEventArgs(this)); };
            Visibility.PropertyChanged += (sender, e) => { VisibilityChangedEvent?.Invoke(this, new PinChangedEventArgs(this)); };

            // データベースのデータセット
            ID = info.ID;
            Name.Value = info.Name;
            Latitude.Value = info.Latitude;
            Longitude.Value = info.Longitude;
            IsVisited.Value = info.IsVisited;
            Notice.Value = info.Notice;

            // ピンとしてのデータセット
            Type = PinType.Place;
        }

        /// <summary>
        /// 道の駅データ更新によるピンデータ更新
        /// </summary>
        /// <param name="info">道の駅データ</param>
        public void UpdateFromMichiNoEkiInfo(MichiNoEkiInfo info)
        {
            Name.Value = info.Name;
            Latitude.Value = info.Latitude;
            Longitude.Value = info.Longitude;
            IsVisited.Value = info.IsVisited;
            Notice.Value = info.Notice;
            IsVisited.Value = info.IsVisited;
            Notice.Value = info.Notice;
        }
    }
}
