using CoreLocation;
using MapKit;

namespace RoadsideStationApp
{
    /// <summary>
    /// 道の駅Annotation
    /// </summary>
    public class MichiNoEkiAnnotation : MKPointAnnotation
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// ピンの色
        /// </summary>
        public Color PinColor { get; set; }

        /// <summary>
        /// ピンの表示有無
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// 訪問状態
        /// </summary>
        public bool IsVisited { get; set; }

        /// <summary>
        /// 注意事項あり
        /// </summary>
        public bool Notice { get; set; }

        /// <summary>
        /// 詳細ボタン押下時ハンドラ
        /// </summary>
        public Action<int>? DetailButtonClickHandler { get; private set; }

        /// <summary>
        /// 訪問ボタン押下時ハンドラ
        /// </summary>
        public Action<int>? VisitedButtonClickHandler { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pin">ピン</param>
        /// <param name="detailButtonClickHandler">詳細ボタン押下時ハンドラ</param>
        /// <param name="visitedButtonClickHandler">訪問ボタン押下時ハンドラ</param>
        public MichiNoEkiAnnotation(MichiNoEkiPin pin, Action<int>? detailButtonClickHandler, Action<int>? visitedButtonClickHandler)
        {
            // 各データを格納
            ID = pin.ID;
            Coordinate = new CLLocationCoordinate2D(pin.Latitude.Value, pin.Longitude.Value);
            Title = pin.Name.Value;
            PinColor = pin.PinColor.Value;
            IsVisited = pin.IsVisited.Value;
            Notice = pin.Notice.Value;
            DetailButtonClickHandler = detailButtonClickHandler;
            VisitedButtonClickHandler = visitedButtonClickHandler;
        }
    }
}