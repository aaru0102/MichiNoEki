using ObservableCollections;
using Microsoft.Maui.Maps;

namespace RoadsideStationApp
{
    /// <summary>
    /// 道の駅マップ
    /// </summary>
    public class MichiNoEkiMap : Microsoft.Maui.Controls.Maps.Map
    {
        /// <summary>
        /// 道の駅ピン
        /// </summary>
        public ObservableDictionary<int, MichiNoEkiPin> MichiNoEkiPins
        {
            get => (ObservableDictionary<int, MichiNoEkiPin>)GetValue(MichiNoEkiPinsProperty);
            set => SetValue(MichiNoEkiPinsProperty, value);
        }

        /// <summary>
        /// ピンの詳細ボタン押下時ハンドラ
        /// </summary>
        public Action<int>? DetailButtonClickHandler
        {
            get => (Action<int>?)GetValue(DetailButtonClickHandlerProperty);
            set => SetValue(DetailButtonClickHandlerProperty, value);
        }

        /// <summary>
        /// ピンの訪問ボタン押下時ハンドラ
        /// </summary>
        public Action<int>? VisitedButtonClickHandler
        {
            get => (Action<int>?)GetValue(VisitedButtonClickHandlerProperty);
            set => SetValue(VisitedButtonClickHandlerProperty, value);
        }

        /// <summary>
        /// MichiNoEkiPinsをBindablePropertyにする
        /// </summary>
        public static readonly BindableProperty MichiNoEkiPinsProperty =
            BindableProperty.Create(
                nameof(MichiNoEkiPins),
                typeof(ObservableDictionary<int, MichiNoEkiPin>),
                typeof(MichiNoEkiMap),
                new ObservableDictionary<int, MichiNoEkiPin>(),
                propertyChanged: OnMichiNoEkiPinsChanged);

        // DetailButtonClickHandlerをBindablePropertyにする
        public static readonly BindableProperty DetailButtonClickHandlerProperty =
            BindableProperty.Create(
                nameof(DetailButtonClickHandler),
                typeof(Action<int>),
                typeof(MichiNoEkiMap),
                default(Action<int>));

        // VisitedButtonClickHandlerをBindablePropertyにする
        public static readonly BindableProperty VisitedButtonClickHandlerProperty =
            BindableProperty.Create(
                nameof(VisitedButtonClickHandler),
                typeof(Action<int>),
                typeof(MichiNoEkiMap),
                default(Action<int>));

        /// <summary>
        /// コンストラクタ
        /// XAML用に公開しているが使わないで！！！(Instanceを使う)
        /// </summary>
        public MichiNoEkiMap()
        {
            // マップ設定
            MapType = MapType.Street;
            IsShowingUser = true;

            // 現在地にマップの中心を移動
            _ = SetMapToCurrentLocation();
        }

        /// <summary>
        /// 現在地にマップを移動
        /// </summary>
        /// <returns>Task</returns>
        private async Task SetMapToCurrentLocation()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    MoveToRegion(MapSpan.FromCenterAndRadius(new Location(location.Latitude, location.Longitude), Distance.FromMiles(1)));
                }
            }
            catch (Exception)
            {
                // Handle exceptions
            }
        }

        /// <summary>
        /// ピンの追加削除時処理
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void OnCustomPinsChanged(in NotifyCollectionChangedEventArgs<KeyValuePair<int, MichiNoEkiPin>> e)
        {
            // カスタムハンドラーが登録されていることを確認
            if (Handler is MichiNoEkiMapHandler handler)
            {
                // ピンの追加の場合
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    // ハンドラーのAnnotation追加メソッドをコール
                    handler.AddAnnotation(e.NewItem.Value, DetailButtonClickHandler, VisitedButtonClickHandler);

                    // 各プロパティ変化時のメソッド登録
                    e.NewItem.Value.NameChangedEvent += ChangedPinName;
                    e.NewItem.Value.LocationChangedEvent += ChangedPinLocation;
                    e.NewItem.Value.PinColorChangedEvent += ChangedPinColor;
                    e.NewItem.Value.IsVisitedChangedEvent += ChangedPinIsVisited;
                    e.NewItem.Value.NoticeChangedEvent += ChangedPinNotice;
                    e.NewItem.Value.VisibilityChangedEvent += ChangedPinVisivility;
                }

                // ピンの削除
                else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                {
                    // ピンの削除処理
                    handler.RemoveAnnotation(e.OldItem.Key);
                }
            }
        }

        /// <summary>
        /// ピンの名称更新処理
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void ChangedPinName(object? sender, PinChangedEventArgs e)
        {
            // カスタムハンドラーが登録されていることを確認
            if (Handler is MichiNoEkiMapHandler handler)
            {
                // ハンドラーのピンの位置を変更するメソッドをコール
                handler.UpdatePinTitle(e.Pin.ID, e.Pin.Name.Value);
            }
        }

        /// <summary>
        /// ピンの場所更新処理
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void ChangedPinLocation(object? sender, PinChangedEventArgs e)
        {
            // カスタムハンドラーが登録されていることを確認
            if (Handler is MichiNoEkiMapHandler handler)
            {
                // ハンドラーのピンの位置を変更するメソッドをコール
                handler.UpdatePinLocation(e.Pin.ID, e.Pin.Latitude.Value, e.Pin.Longitude.Value);
            }
        }

        /// <summary>
        /// ピンの色更新処理
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void ChangedPinColor(object? sender, PinChangedEventArgs e)
        {
            // カスタムハンドラーが登録されていることを確認
            if (Handler is MichiNoEkiMapHandler handler)
            {
                // ハンドラーのピンの色を変更するメソッドをコール
                handler.UpdatePinColor(e.Pin.ID, e.Pin.PinColor.Value);
            }
        }

        /// <summary>
        /// ピンの訪問状態更新処理
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void ChangedPinIsVisited(object? sender, PinChangedEventArgs e)
        {
            // カスタムハンドラーが登録されていることを確認
            if (Handler is MichiNoEkiMapHandler handler)
            {
                // ハンドラーのピンの表示状態を変更するメソッドをコール
                handler.UpdatePinVisitedButton(e.Pin.ID, e.Pin.IsVisited.Value);
            }
        }

        /// <summary>
        /// ピンの注意事項有無更新処理
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void ChangedPinNotice(object? sender, PinChangedEventArgs e)
        {
            // カスタムハンドラーが登録されていることを確認
            if (Handler is MichiNoEkiMapHandler handler)
            {
                // ハンドラーのピンの画像を変更するメソッドをコール
                handler.UpdatePinImage(e.Pin.ID, e.Pin.Notice.Value);
            }
        }

        /// <summary>
        /// ピンの表示状態更新処理
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void ChangedPinVisivility(object? sender, PinChangedEventArgs e)
        {
            // カスタムハンドラーが登録されていることを確認
            if (Handler is MichiNoEkiMapHandler handler)
            {
                // ハンドラーのピンの表示状態を変更するメソッドをコール
                handler.UpdatePinVisibility(e.Pin.ID, e.Pin.Visibility.Value);
            }
        }

        /// <summary>
        /// MichiNoEkiPinsのインスタンスが変わったときの処理
        /// </summary>
        /// <param name="bindable">MichiNoEkiMapのインスタンス</param>
        /// <param name="oldValue">MichiNoEkiPinsの変更前のインスタンス</param>
        /// <param name="newValue">MichiNoEkiPinsの変更後のインスタンス</param>
        private static void OnMichiNoEkiPinsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is MichiNoEkiMap map)
            {
                // 古いコレクションからイベントの登録を解除
                if (oldValue is ObservableDictionary<int, MichiNoEkiPin> oldPins)
                {
                    oldPins.CollectionChanged -= map.OnCustomPinsChanged;
                }

                // 新しいコレクションにイベントを登録
                if (newValue is ObservableDictionary<int, MichiNoEkiPin> newPins)
                {
                    newPins.CollectionChanged += map.OnCustomPinsChanged;
                }
            }
        }
    }
}
