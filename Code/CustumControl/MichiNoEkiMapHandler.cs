#if __IOS__ || MACCATALYST
using PlatformView = Microsoft.Maui.Maps.Platform.MauiMKMapView;
using MapKit;
#endif

using Microsoft.Maui.Platform;
using Microsoft.Maui.Maps.Handlers;
using UIKit;
using Microsoft.Maui.Controls.Maps;

namespace RoadsideStationApp
{
    /// <summary>
    /// 道の駅マップ用のハンドラー
    /// </summary>
    public partial class MichiNoEkiMapHandler : MapHandler
    {
        private Dictionary<int, MichiNoEkiAnnotation> _annotations = new Dictionary<int, MichiNoEkiAnnotation>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MichiNoEkiMapHandler() : base(Mapper, CommandMapper) { }

        /// <summary>
        /// Annotationの追加
        /// </summary>
        /// <param name="pin">ピン</param>
        /// <param name="detailButtonClickHandler">詳細ボタン押下時ハンドラ</param>
        /// <param name="visitedButtonClickHandler">訪問ボタン押下時ハンドラ</param>
        public void AddAnnotation(MichiNoEkiPin pin, Action<int>? detailButtonClickHandler, Action<int>? visitedButtonClickHandler)
        {
            // マップViewがnullの場合はスルー(非接続時?)
            if (PlatformView == null) return;

            // Annotationの生成
            var annotation = new MichiNoEkiAnnotation(
                pin,
                detailButtonClickHandler,
                visitedButtonClickHandler);

            // マップViewにAnnotationを追加
            _annotations.Add(pin.ID, annotation);
            PlatformView.AddAnnotation(annotation);
        }

        /// <summary>
        /// Annotationの削除
        /// </summary>
        /// <param name="pinId">ピンID</param>
        public void RemoveAnnotation(int pinId)
        {
            // マップViewがnullの場合はスルー(非接続時?)
            if (PlatformView == null) return;

            // Annotationをディクショナリーから取得
            MichiNoEkiAnnotation annotation = _annotations[pinId];

            // マップViewからAnnotationから削除
            _annotations.Remove(pinId);
            PlatformView.RemoveAnnotation(annotation);
        }

        /// <summary>
        /// ピンのタイトルを変更する
        /// </summary>
        /// <param name="pinId">ピンID</param>
        /// <param name="newTitle">色</param>
        public void UpdatePinTitle(int pinId, string newTitle)
        {
            // マップViewがnullの場合はスルー(非接続時?)
            if (PlatformView == null) return;

            // Annotationをディクショナリーから取得
            MichiNoEkiAnnotation annotation = _annotations[pinId];

            // ピン(Annotation)の色を変更する
            annotation.Title = newTitle;
        }

        /// <summary>
        /// ピンの位置を変更する
        /// </summary>
        /// <param name="pinId">ピンID</param>
        /// <param name="newLatitude">緯度</param>
        /// <param name="newLongitude">経度</param>
        public void UpdatePinLocation(int pinId, double newLatitude, double newLongitude)
        {
            // マップViewがnullの場合はスルー(非接続時?)
            if (PlatformView == null) return;

            // Annotationをディクショナリーから取得
            MichiNoEkiAnnotation annotation = _annotations[pinId];

            // ピン(Annotation)の場所を変更する
            annotation.Coordinate = new CoreLocation.CLLocationCoordinate2D(newLatitude, newLongitude);
        }

        /// <summary>
        /// ピンの色を変更する
        /// </summary>
        /// <param name="pinId">ピンID</param>
        /// <param name="newColor">色</param>
        public void UpdatePinColor(int pinId, Color newColor)
        {
            // マップViewがnullの場合はスルー(非接続時?)
            if (PlatformView == null) return;

            // Annotationをディクショナリーから取得
            MichiNoEkiAnnotation annotation = _annotations[pinId];

            // ピン(Annotation)の色を変更する
            annotation.PinColor = newColor;

            // AnnotationからピンのViewを取得
            var pinView = PlatformView.ViewForAnnotation(annotation) as MKMarkerAnnotationView;

            // ピンのViewが取得出来たら
            if (pinView != null)
            {
                // ピンの色を変更する
                pinView.MarkerTintColor = newColor.ToPlatform();

                // ピンが現在選択されているか確認
                bool isAnnotationSelected = PlatformView.SelectedAnnotations.Contains(annotation);
                if (isAnnotationSelected == false)
                {
                    _annotations.Remove(pinId);
                    PlatformView.RemoveAnnotation(annotation);
                    _annotations.Add(pinId, annotation);
                    PlatformView.AddAnnotation(annotation);
                }
            }
        }

        /// <summary>
        /// ピンの表示状態を変更する
        /// </summary>
        /// <param name="pinId">ピンID</param>
        /// <param name="isVisible">表示状態</param>
        public void UpdatePinVisibility(int pinId, bool isVisible)
        {
            // マップViewがnullの場合はスルー(非接続時?)
            if (PlatformView == null) return;

            // Annotationをディクショナリーから取得
            MichiNoEkiAnnotation annotation = _annotations[pinId];

            // ピン(Annotation)の表示有無を変更する
            annotation.IsVisible = isVisible;

            // AnnotationからピンのViewを取得
            var pinView = PlatformView.ViewForAnnotation(annotation) as MKMarkerAnnotationView;

            // ピンのViewが取得出来たら
            if (pinView != null)
            {
                // ピンの表示状態を変更する
                pinView.Hidden = !isVisible;
            }
        }

        /// <summary>
        /// ピンの訪問状態を変更する
        /// </summary>
        /// <param name="pinId">ピンID</param>
        /// <param name="IsVisited">訪問済み</param>
        public void UpdatePinVisitedButton(int pinId, bool IsVisited)
        {
            // マップViewがnullの場合はスルー(非接続時?)
            if (PlatformView == null) return;

            // Annotationをディクショナリーから取得
            MichiNoEkiAnnotation annotation = _annotations[pinId];

            // ピン(Annotation)の訪問有無を変更する
            annotation.IsVisited = IsVisited;

            // AnnotationからピンのViewを取得
            var pinView = PlatformView.ViewForAnnotation(annotation) as MKMarkerAnnotationView;

            // ピンのViewが取得出来たら
            if (pinView != null)
            {
                // ピンの訪問状態に応じた画像情報を生成する
                string imagePath = "star_icon.png";
                if (IsVisited == true)
                {
                    imagePath = "star_icon_visited.png";
                }
                UIImage starImage = UIImage.FromBundle(imagePath)!;
                var imageView = new UIImageView(starImage)
                {
                    Frame = pinView.LeftCalloutAccessoryView!.Frame,
                    ContentMode = pinView.LeftCalloutAccessoryView!.ContentMode,
                    UserInteractionEnabled = true,
                };
                imageView.AddGestureRecognizer(
                    new UITapGestureRecognizer(() =>
                    {
                        annotation.VisitedButtonClickHandler?.Invoke(annotation.ID);
                    }));

                // 画像情報をセット
                pinView.LeftCalloutAccessoryView = imageView;
            }
        }

        /// <summary>
        /// ピンの画像を標準から差し替える
        /// </summary>
        /// <param name="pinId">ピンID</param>
        /// <param name="isValid">有効</param>
        public void UpdatePinImage(int pinId, bool isValid)
        {
            // マップViewがnullの場合はスルー(非接続時?)
            if (PlatformView == null) return;

            // Annotationをディクショナリーから取得
            MichiNoEkiAnnotation annotation = _annotations[pinId];

            // ピン(Annotation)の注意事項有無を変更する
            annotation.Notice = isValid;

            // AnnotationからピンのViewを取得
            var pinView = PlatformView.ViewForAnnotation(annotation) as MKMarkerAnnotationView;

            // ピンのViewが取得出来たら
            if (pinView != null)
            {
                if (isValid == true)
                {
                    // ピンの画像を注意喚起用に変更する
                    pinView.GlyphImage = UIImage.FromBundle("notice.png");
                }
                else
                {
                    // ピンの画像をデフォルトに変更する
                    pinView.GlyphImage = null;
                }
            }
        }

        /// <summary>
        /// iOSのマップViewに接続した時の処理
        /// </summary>
        /// <param name="platformView">iOSのマップView</param>
        protected override void ConnectHandler(PlatformView platformView)
        {
            // ベースをコール
            base.ConnectHandler(platformView);

            // マップのユーザーインターフェイススタイルをライトモードに固定
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                platformView.OverrideUserInterfaceStyle = UIUserInterfaceStyle.Light;
            }

            // Annotationが読み込まれるされる際の処理を登録
            platformView.GetViewForAnnotation -= GetCustomPinView;
            platformView.GetViewForAnnotation += GetCustomPinView;
        }

        /// <summary>
        /// ピンの見た目をカスタマイズする
        /// </summary>
        /// <param name="mapView"></param>
        /// <param name="annotation"></param>
        /// <returns></returns>
        private MKAnnotationView GetCustomPinView(MKMapView mapView, IMKAnnotation annotation)
        {
            // 現在地表示のAnnotationはカスタムしない
            if (annotation.Title == "My Location")
                return null!;

            // 識別子を設定
            const string annotationIdentifier = "MichiNoEkiPin";

            // 識別子を基に再利用可能なMKMarkerAnnotationViewを取得
            MKMarkerAnnotationView? pinView = mapView.DequeueReusableAnnotation(annotationIdentifier) as MKMarkerAnnotationView;

            // 初回、基本View作成時
            if (pinView == null)
            {
                // バルーン状の見た目を採用
                pinView = new MKMarkerAnnotationView(annotation, annotationIdentifier)
                {
                    // ピン選択時のふきだしを有効にする
                    CanShowCallout = true,

                    // ふきだし内の右側の詳細ボタンを有効にする
                    RightCalloutAccessoryView = UIButton.FromType(UIButtonType.DetailDisclosure)
                };
            }

            // 基本View作成済みの場合
            else
            {
                // ViewのAnnotationを本Annotationで初期化
                pinView.Annotation = annotation;
            }

            // ViewにAnnotationのカスタム情報をセット
            if (annotation is MichiNoEkiAnnotation customAnnotation)
            {
                // 表示状態
                pinView.Hidden = !customAnnotation.IsVisible;

                // ピンの色
                pinView.MarkerTintColor = customAnnotation.PinColor.ToPlatform();

                // 表示優先度を上げる
                pinView.DisplayPriority = 1000;

                // 詳細ボタン押下時ハンドラ登録
                pinView.RightCalloutAccessoryView?.AddGestureRecognizer(
                    new UITapGestureRecognizer(() =>
                    {
                        customAnnotation.DetailButtonClickHandler?.Invoke(customAnnotation.ID);
                    }));

                // 訪問状態に応じた画像情報を生成する
                string imagePath = "star_icon.png";
                if (customAnnotation.IsVisited == true)
                {
                    imagePath = "star_icon_visited.png";
                }
                UIImage starImage = UIImage.FromBundle(imagePath)!;
                var imageView = new UIImageView(starImage)
                {
                    Frame = new CoreGraphics.CGRect(0, 0, 24, 24),
                    ContentMode = UIViewContentMode.ScaleAspectFit,
                    UserInteractionEnabled = true,
                };

                // 画像情報に訪問ボタン押下時ハンドラ登録
                imageView.AddGestureRecognizer(
                    new UITapGestureRecognizer(() =>
                    {
                        customAnnotation.VisitedButtonClickHandler?.Invoke(customAnnotation.ID);
                    }));

                // ポップアップの左に表示される画像を設定
                pinView.LeftCalloutAccessoryView = imageView;

                // 注意事項ありの場合は画像をビックリマークに差し替える
                if (customAnnotation.Notice == true)
                {
                    pinView.GlyphImage = UIImage.FromBundle("notice.png");
                }
            }

            return pinView;
        }

        ///// <summary>
        ///// IDの一致するAnnotationを検索する
        ///// </summary>
        ///// <param name="pinId">ピンID</param>
        ///// <returns>Annotation</returns>
        //private MichiNoEkiAnnotation? GetAnnotationForPin(int pinId)
        //{
        //    // マップViewにあるAnnotationから検索
        //    foreach (var annotation in PlatformView.Annotations)
        //    {
        //        // IDが一致したAnnotationを返す
        //        if (annotation is MichiNoEkiAnnotation customAnnotation && customAnnotation.ID == pinId)
        //        {
        //            return customAnnotation;
        //        }
        //    }

        //    // Annotationが見つからなかった場合はnullを返す
        //    return null;
        //}
    }
}
