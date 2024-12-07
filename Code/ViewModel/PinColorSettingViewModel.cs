using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    public class PinColorSettingViewModel
    {
        private (Type Type, object Kind) _settingNowData;
        private bool _nowLoading = false;

        /// <summary>
        /// ピンカラー設定データ管理Model
        /// </summary>
        private PinColorSettingDataModel _pinColorSettingDataModel;

        /// <summary>
        /// 有効なピンカラー設定
        /// </summary>
        public AutoNotifyProperty<PinColorSettingID> ValidPinColorSetting { get; set; } = new AutoNotifyProperty<PinColorSettingID>(PinColorSettingID.Visited);

        /// <summary>
        /// 詳細メニューを開いているか(訪問状態)
        /// </summary>
        public AutoNotifyProperty<bool> VisitedDetailMenuOpened { get; set; } = new AutoNotifyProperty<bool>(false);

        /// <summary>
        /// 詳細メニューを開いているか(開館時間)
        /// </summary>
        public AutoNotifyProperty<bool> OpenTimeDetailMenuOpened { get; set; } = new AutoNotifyProperty<bool>(false);

        /// <summary>
        /// 詳細メニューを開いているか(閉館時間)
        /// </summary>
        public AutoNotifyProperty<bool> CloseTimeDetailMenuOpened { get; set; } = new AutoNotifyProperty<bool>(false);

        /// <summary>
        /// 詳細メニューを開いているか(定休日)
        /// </summary>
        public AutoNotifyProperty<bool> CloseDayDetailMenuOpened { get; set; } = new AutoNotifyProperty<bool>(false);

        /// <summary>
        /// ピンカラー設定対象月
        /// </summary>
        public AutoNotifyProperty<int> PinColorMonth { get; set; } = new AutoNotifyProperty<int>(5);

        /// <summary>
        /// 未オープン ピンカラー
        /// </summary>
        public AutoNotifyProperty<Color> NotOpendPinColor { get; set; } = new AutoNotifyProperty<Color>(Colors.White);

        /// <summary>
        /// スタンプ24時間押下OK ピンカラー
        /// </summary>
        public AutoNotifyProperty<Color> StampAllTimeOKPinColor { get; set; } = new AutoNotifyProperty<Color>(Colors.White);

        /// <summary>
        /// ピンカラー設定 訪問状態 設定内容
        /// </summary>
        public Dictionary<PinColorSettingVisitedID, PinColorSettingVisitedDataListItem> VisitedDataDic { get; set; } = new Dictionary<PinColorSettingVisitedID, PinColorSettingVisitedDataListItem>();

        /// <summary>
        /// ピンカラー設定 開館時間 設定内容
        /// </summary>
        public Dictionary<PinColorSettingTimeID, PinColorSettingTimeDataListItem> OpenTimeDataDic { get; set; } = new Dictionary<PinColorSettingTimeID, PinColorSettingTimeDataListItem>();

        /// <summary>
        /// ピンカラー設定 開館時間 ラストデータ 設定内容
        /// </summary>
        public PinColorSettingTimeDataListItem OpenTimeLastData { get; set; }

        /// <summary>
        /// ピンカラー設定 閉館時間 設定内容
        /// </summary>
        public Dictionary<PinColorSettingTimeID, PinColorSettingTimeDataListItem> CloseTimeDataDic { get; set; } = new Dictionary<PinColorSettingTimeID, PinColorSettingTimeDataListItem>();

        /// <summary>
        /// ピンカラー設定 閉館時間 ファーストデータ 設定内容
        /// </summary>
        public PinColorSettingTimeDataListItem CloseTimeFirstData { get; set; }

        /// <summary>
        /// ピンカラー設定 定休日 設定内容
        /// </summary>
        public Dictionary<PinColorSettingCloseDayID, PinColorSettingCloseDayDataListItem> CloseDayDataDic { get; set; } = new Dictionary<PinColorSettingCloseDayID, PinColorSettingCloseDayDataListItem>();

        /// <summary>
        /// 選択カラー
        /// </summary>
        public AutoNotifyProperty<Color> SelectedColor { get; set; } = new AutoNotifyProperty<Color>(Colors.White);

        /// <summary>
        /// カラーピッカー表示状態
        /// </summary>
        public AutoNotifyProperty<bool> ColorPickerVisible { get; set; } = new AutoNotifyProperty<bool>(false);

        /// <summary>
        /// 詳細メニューオープンコマンド
        /// </summary>
        public DelegateCommand OpenDetailMenuCommand { get; set; }

        /// <summary>
        /// カラー変更コマンド
        /// </summary>
        public DelegateCommand ChangeColorCommand { get; set; }

        /// <summary>
        /// カラー変更(訪問状態)コマンド
        /// </summary>
        public DelegateCommand ChangeVisitedColorCommand { get; set; }

        /// <summary>
        /// カラー変更(開館時間)コマンド
        /// </summary>
        public DelegateCommand ChangeOpenTimeColorCommand { get; set; }

        /// <summary>
        /// カラー変更(閉館時間)コマンド
        /// </summary>
        public DelegateCommand ChangeCloseTimeColorCommand { get; set; }

        /// <summary>
        /// カラー変更(定休日)コマンド
        /// </summary>
        public DelegateCommand ChangeCloseDayColorCommand { get; set; }

        /// <summary>
        /// カラー保存コマンド
        /// </summary>
        public DelegateCommand SaveColorCommand { get; set; }

        /// <summary>
        /// カラー選択キャンセルコマンド
        /// </summary>
        public DelegateCommand SelectColorCancelCommand { get; set; }

        /// <summary>
        /// 有効なピンカラー設定変更コマンド
        /// </summary>
        public DelegateCommand ChangeValidPinColorSettingCommand { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PinColorSettingViewModel()
        {
            // 訪問状態
            VisitedDataDic.Add(PinColorSettingVisitedID.NotVisited, new PinColorSettingVisitedDataListItem(PinColorSettingVisitedID.NotVisited, "未訪問"));
            VisitedDataDic.Add(PinColorSettingVisitedID.Visited, new PinColorSettingVisitedDataListItem(PinColorSettingVisitedID.Visited, "訪問済み"));

            // 開館時間
            OpenTimeDataDic.Add(PinColorSettingTimeID.Time1, new PinColorSettingTimeDataListItem(PinColorSettingTimeID.Time1, TimeSpan.Parse("23:53")));
            OpenTimeDataDic.Add(PinColorSettingTimeID.Time2, new PinColorSettingTimeDataListItem(PinColorSettingTimeID.Time2, TimeSpan.Parse("23:54")));
            OpenTimeDataDic.Add(PinColorSettingTimeID.Time3, new PinColorSettingTimeDataListItem(PinColorSettingTimeID.Time3, TimeSpan.Parse("23:55")));
            OpenTimeDataDic.Add(PinColorSettingTimeID.Time4, new PinColorSettingTimeDataListItem(PinColorSettingTimeID.Time4, TimeSpan.Parse("23:56")));
            OpenTimeDataDic.Add(PinColorSettingTimeID.Time5, new PinColorSettingTimeDataListItem(PinColorSettingTimeID.Time5, TimeSpan.Parse("23:57")));
            OpenTimeDataDic.Add(PinColorSettingTimeID.Time6, new PinColorSettingTimeDataListItem(PinColorSettingTimeID.Time6, TimeSpan.Parse("23:58")));
            OpenTimeLastData = new PinColorSettingTimeDataListItem(PinColorSettingTimeID.Time7, TimeSpan.Parse("23:59"));

            // 閉館時間
            CloseTimeFirstData = new PinColorSettingTimeDataListItem(PinColorSettingTimeID.Time1, TimeSpan.Parse("00:00"));
            CloseTimeDataDic.Add(PinColorSettingTimeID.Time2, new PinColorSettingTimeDataListItem(PinColorSettingTimeID.Time2, TimeSpan.Parse("00:01")));
            CloseTimeDataDic.Add(PinColorSettingTimeID.Time3, new PinColorSettingTimeDataListItem(PinColorSettingTimeID.Time3, TimeSpan.Parse("00:02")));
            CloseTimeDataDic.Add(PinColorSettingTimeID.Time4, new PinColorSettingTimeDataListItem(PinColorSettingTimeID.Time4, TimeSpan.Parse("00:03")));
            CloseTimeDataDic.Add(PinColorSettingTimeID.Time5, new PinColorSettingTimeDataListItem(PinColorSettingTimeID.Time5, TimeSpan.Parse("00:04")));
            CloseTimeDataDic.Add(PinColorSettingTimeID.Time6, new PinColorSettingTimeDataListItem(PinColorSettingTimeID.Time6, TimeSpan.Parse("00:05")));
            CloseTimeDataDic.Add(PinColorSettingTimeID.Time7, new PinColorSettingTimeDataListItem(PinColorSettingTimeID.Time7, TimeSpan.Parse("00:06")));

            // 定休日
            CloseDayDataDic.Add(PinColorSettingCloseDayID.Monday, new PinColorSettingCloseDayDataListItem(PinColorSettingCloseDayID.Monday, "月曜日"));
            CloseDayDataDic.Add(PinColorSettingCloseDayID.Tuesday, new PinColorSettingCloseDayDataListItem(PinColorSettingCloseDayID.Tuesday, "火曜日"));
            CloseDayDataDic.Add(PinColorSettingCloseDayID.Wednesday, new PinColorSettingCloseDayDataListItem(PinColorSettingCloseDayID.Wednesday, "水曜日"));
            CloseDayDataDic.Add(PinColorSettingCloseDayID.Thursday, new PinColorSettingCloseDayDataListItem(PinColorSettingCloseDayID.Thursday, "木曜日"));
            CloseDayDataDic.Add(PinColorSettingCloseDayID.Friday, new PinColorSettingCloseDayDataListItem(PinColorSettingCloseDayID.Friday, "金曜日"));
            CloseDayDataDic.Add(PinColorSettingCloseDayID.Saturday, new PinColorSettingCloseDayDataListItem(PinColorSettingCloseDayID.Saturday, "土曜日"));
            CloseDayDataDic.Add(PinColorSettingCloseDayID.Sunday, new PinColorSettingCloseDayDataListItem(PinColorSettingCloseDayID.Sunday, "日曜日"));
            CloseDayDataDic.Add(PinColorSettingCloseDayID.Nothing, new PinColorSettingCloseDayDataListItem(PinColorSettingCloseDayID.Nothing, "定休日なし"));

            // コマンド
            OpenDetailMenuCommand = new DelegateCommand(OnOpenDetailMenuCommand);
            ChangeColorCommand = new DelegateCommand(OnChangeColorCommand);
            ChangeVisitedColorCommand = new DelegateCommand(OnChangeVisitedColorCommand);
            ChangeOpenTimeColorCommand = new DelegateCommand(OnChangeOpenTimeColorCommand);
            ChangeCloseTimeColorCommand = new DelegateCommand(OnChangeCloseTimeColorCommand);
            ChangeCloseDayColorCommand = new DelegateCommand(OnChangeCloseDayColorCommand);
            SaveColorCommand = new DelegateCommand(OnSaveColorCommand);
            SelectColorCancelCommand = new DelegateCommand(OnSelectColorCancelCommand);
            ChangeValidPinColorSettingCommand = new DelegateCommand(OnChangeValidPinColorSettingCommand);

            // 開館時間閉館時間の設定値変更時の処理セット
            foreach (var id in OpenTimeDataDic.Keys)
            {
                OpenTimeDataDic[id].IsValid.PropertyChanged += (s, e) => { ChangeOpenCloseValid(PinColorSettingID.OpenTime, id); };
                OpenTimeDataDic[id].Time.PropertyChanged += (s, e) => { ChangeOpenCloseTime(PinColorSettingID.OpenTime, id); };
            }
            foreach (var id in CloseTimeDataDic.Keys)
            {
                CloseTimeDataDic[id].IsValid.PropertyChanged += (s, e) => { ChangeOpenCloseValid(PinColorSettingID.CloseTime, id); };
                CloseTimeDataDic[id].Time.PropertyChanged += (s, e) => { ChangeOpenCloseTime(PinColorSettingID.CloseTime, id); };
            }

            // ピンカラー設定対象月変更時の処理セット
            PinColorMonth.PropertyChanged += (s, e) => { ChangePinColorMonth(); };

            // ピンカラー設定データ管理Model取得
            _pinColorSettingDataModel = App.Instance.PinColorSettingDataModel;

            // ピンカラー設定データ管理Modelのイベント捕捉
            _pinColorSettingDataModel.LoadedPinColorSettingInfoEvent += OnLoadedPinColorSettingInfoEvent;
        }

        /// <summary>
        /// ピンカラー設定ロードイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void OnLoadedPinColorSettingInfoEvent(object? sender, LoadedPinColorSettingInfoEventArgs e)
        {
            _nowLoading = true;

            ValidPinColorSetting.Value = e.ValidPinColorSetting;
            PinColorMonth.Value = e.PinColorMonth;
            NotOpendPinColor.Value = e.NotOpendPinColor;
            StampAllTimeOKPinColor.Value = e.StampAllTimeOKPinColor;

            foreach (var item in e.VisitedDataDic.Values)
            {
                VisitedDataDic[item.ID].Color.Value = item.Color;
            }

            foreach (var item in e.OpenTimeDataDic.Values)
            {
                if (item.ID == PinColorSettingTimeID.Time7)
                {
                    OpenTimeLastData.IsValid.Value = item.IsValid;
                    OpenTimeLastData.Time.Value = item.Time;
                    OpenTimeLastData.Color.Value = item.Color;
                }
                else
                {
                    OpenTimeDataDic[item.ID].IsValid.Value = item.IsValid;
                    OpenTimeDataDic[item.ID].Time.Value = item.Time;
                    OpenTimeDataDic[item.ID].Color.Value = item.Color;
                }
            }

            foreach (var item in e.CloseTimeDataDic.Values)
            {
                if (item.ID == PinColorSettingTimeID.Time1)
                {
                    CloseTimeFirstData.IsValid.Value = item.IsValid;
                    CloseTimeFirstData.Time.Value = item.Time;
                    CloseTimeFirstData.Color.Value = item.Color;
                }
                else
                {
                    CloseTimeDataDic[item.ID].IsValid.Value = item.IsValid;
                    CloseTimeDataDic[item.ID].Time.Value = item.Time;
                    CloseTimeDataDic[item.ID].Color.Value = item.Color;
                }
            }

            foreach (var item in e.CloseDayDataDic.Values)
            {
                CloseDayDataDic[item.ID].Color.Value = item.Color;
            }

            _nowLoading = false;
        }

        /// <summary>
        /// 詳細メニューオープンコマンドハンドラ
        /// </summary>
        /// <param name="param">コマンド引数</param>
        private void OnOpenDetailMenuCommand(object? param)
        {
            if (param is PinColorSettingID id)
            {
                switch (id)
                {
                    case PinColorSettingID.Visited:
                        VisitedDetailMenuOpened.Value = !VisitedDetailMenuOpened.Value;
                        break;

                    case PinColorSettingID.OpenTime:
                        OpenTimeDetailMenuOpened.Value = !OpenTimeDetailMenuOpened.Value;
                        break;

                    case PinColorSettingID.CloseTime:
                        CloseTimeDetailMenuOpened.Value = !CloseTimeDetailMenuOpened.Value;
                        break;

                    case PinColorSettingID.CloseDay:
                        CloseDayDetailMenuOpened.Value = !CloseDayDetailMenuOpened.Value;
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// カラー変更コマンドハンドラ
        /// </summary>
        /// <param name="param">コマンド引数</param>
        private void OnChangeColorCommand(object? param)
        {
            if (param is string kind)
            {
                _settingNowData = (typeof(Color), kind);

                switch (kind)
                {
                    case "未オープン":
                        SelectedColor.Value = NotOpendPinColor.Value;
                        break;

                    case "24時間スタンプ押印OK":
                        SelectedColor.Value = StampAllTimeOKPinColor.Value;
                        break;

                    default:
                        break;
                }
            }

            ColorPickerVisible.Value = true;
        }

        /// <summary>
        /// カラー変更(訪問状態)コマンドハンドラ
        /// </summary>
        /// <param name="param">コマンド引数</param>
        private void OnChangeVisitedColorCommand(object? param)
        {
            if (param is PinColorSettingVisitedID kind)
            {
                _settingNowData = (typeof(PinColorSettingVisitedData), kind);
                SelectedColor.Value = VisitedDataDic[kind].Color.Value;
            }

            ColorPickerVisible.Value = true;
        }

        /// <summary>
        /// カラー変更(開館時間)コマンドハンドラ
        /// </summary>
        /// <param name="param">コマンド引数</param>
        private void OnChangeOpenTimeColorCommand(object? param)
        {
            if (param is PinColorSettingTimeID kind)
            {
                _settingNowData = (typeof(PinColorSettingOpenTimeData), kind);
                if (kind == PinColorSettingTimeID.Time7)
                {
                    SelectedColor.Value = OpenTimeLastData.Color.Value;
                }
                else
                {
                    SelectedColor.Value = OpenTimeDataDic[kind].Color.Value;
                }
            }

            ColorPickerVisible.Value = true;
        }

        /// <summary>
        /// カラー変更(閉館時間)コマンドハンドラ
        /// </summary>
        /// <param name="param">コマンド引数</param>
        private void OnChangeCloseTimeColorCommand(object? param)
        {
            if (param is PinColorSettingTimeID kind)
            {
                _settingNowData = (typeof(PinColorSettingCloseTimeData), kind);
                if (kind == PinColorSettingTimeID.Time1)
                {
                    SelectedColor.Value = CloseTimeFirstData.Color.Value;
                }
                else
                {
                    SelectedColor.Value = CloseTimeDataDic[kind].Color.Value;
                }
            }

            ColorPickerVisible.Value = true;
        }

        /// <summary>
        /// カラー変更(定休日)コマンドハンドラ
        /// </summary>
        /// <param name="param">コマンド引数</param>
        private void OnChangeCloseDayColorCommand(object? param)
        {
            if (param is PinColorSettingCloseDayID kind)
            {
                _settingNowData = (typeof(PinColorSettingCloseDayData), kind);
                SelectedColor.Value = CloseDayDataDic[kind].Color.Value;
            }

            ColorPickerVisible.Value = true;
        }

        /// <summary>
        /// カラー保存コマンドハンドラ
        /// </summary>
        /// <param name="param">コマンド引数</param>
        private void OnSaveColorCommand(object? param)
        {
            if (_settingNowData.Type == typeof(Color))
            {
                if (_settingNowData.Kind is string kind)
                {
                    switch (kind)
                    {
                        case "未オープン":
                            _pinColorSettingDataModel.ChangeNotOpendPinColorSetting(SelectedColor.Value);
                            NotOpendPinColor.Value = SelectedColor.Value;
                            break;

                        case "24時間スタンプ押印OK":
                            _pinColorSettingDataModel.ChangeStampAllTimeOKPinColorSetting(SelectedColor.Value);
                            StampAllTimeOKPinColor.Value = SelectedColor.Value;
                            break;

                        default:
                            break;
                    }
                }
            }
            else if (_settingNowData.Type == typeof(PinColorSettingVisitedData))
            {
                if (_settingNowData.Kind is PinColorSettingVisitedID kind)
                {
                    VisitedDataDic[kind].Color.Value = SelectedColor.Value;
                    SaveVisitedSetting(kind);
                }
            }
            else if (_settingNowData.Type == typeof(PinColorSettingOpenTimeData))
            {
                if (_settingNowData.Kind is PinColorSettingTimeID kind)
                {
                    if (kind == PinColorSettingTimeID.Time7)
                    {
                        OpenTimeLastData.Color.Value = SelectedColor.Value;
                    }
                    else
                    {
                        OpenTimeDataDic[kind].Color.Value = SelectedColor.Value;
                    }
                    SaveOpenTimeSetting(kind);
                }
            }
            else if (_settingNowData.Type == typeof(PinColorSettingCloseTimeData))
            {
                if (_settingNowData.Kind is PinColorSettingTimeID kind)
                {
                    if (kind == PinColorSettingTimeID.Time1)
                    {
                        CloseTimeFirstData.Color.Value = SelectedColor.Value;
                    }
                    else
                    {
                        CloseTimeDataDic[kind].Color.Value = SelectedColor.Value;
                    }
                    SaveCloseTimeSetting(kind);
                }
            }
            else if (_settingNowData.Type == typeof(PinColorSettingCloseDayData))
            {
                if (_settingNowData.Kind is PinColorSettingCloseDayID kind)
                {
                    CloseDayDataDic[kind].Color.Value = SelectedColor.Value;
                    SaveCloseDaySetting(kind);
                }
            }

            ColorPickerVisible.Value = false;
        }

        /// <summary>
        /// カラー選択キャンセルコマンドハンドラ
        /// </summary>
        /// <param name="param">コマンド引数</param>
        private void OnSelectColorCancelCommand(object? param)
        {
            ColorPickerVisible.Value = false;
        }

        /// <summary>
        /// 有効なピンカラー設定変更コマンドハンドラ
        /// </summary>
        /// <param name="param">コマンド引数</param>
        private void OnChangeValidPinColorSettingCommand(object? param)
        {
            if (param is PinColorSettingID id)
            {
                if (id == ValidPinColorSetting.Value)
                {
                    _pinColorSettingDataModel.ChangePinColorSetting(id);
                }
            }
        }

        /// <summary>
        /// 開館時間閉館時間の時間変更時処理
        /// </summary>
        /// <param name="id">ピンカラー設定ID</param>
        /// <param name="timeId">ピンカラー設定タイムID</param>
        private void ChangeOpenCloseTime(PinColorSettingID id, PinColorSettingTimeID timeId)
        {
            if (_nowLoading == true) { return; }

            if (id == PinColorSettingID.OpenTime)
            {
                if (timeId == PinColorSettingTimeID.Time6)
                {
                    if (OpenTimeDataDic[timeId].Time.Value >= TimeSpan.Parse("23:59"))
                    {
                        OpenTimeDataDic[timeId].Time.Value = TimeSpan.Parse("23:58");
                        return;
                    }
                    else if (OpenTimeDataDic[timeId - 1].Time.Value >= OpenTimeDataDic[timeId].Time.Value)
                    {
                        OpenTimeDataDic[timeId].Time.Value = OpenTimeDataDic[timeId - 1].Time.Value + TimeSpan.FromMinutes(1);
                        return;
                    }

                    OpenTimeLastData.Time.Value = OpenTimeDataDic[timeId].Time.Value + TimeSpan.FromMinutes(1);
                    SaveOpenTimeSetting(PinColorSettingTimeID.Time7);
                }
                else if (timeId == PinColorSettingTimeID.Time1)
                {
                    if (OpenTimeDataDic[timeId].Time.Value >= OpenTimeDataDic[timeId + 1].Time.Value)
                    {
                        OpenTimeDataDic[timeId].Time.Value = OpenTimeDataDic[timeId + 1].Time.Value - TimeSpan.FromMinutes(1);
                        return;
                    }
                }
                else
                {
                    if (OpenTimeDataDic[timeId].Time.Value >= OpenTimeDataDic[timeId + 1].Time.Value)
                    {
                        OpenTimeDataDic[timeId].Time.Value = OpenTimeDataDic[timeId + 1].Time.Value - TimeSpan.FromMinutes(1);
                        return;
                    }
                    else if (OpenTimeDataDic[timeId - 1].Time.Value >= OpenTimeDataDic[timeId].Time.Value)
                    {
                        OpenTimeDataDic[timeId].Time.Value = OpenTimeDataDic[timeId - 1].Time.Value + TimeSpan.FromMinutes(1);
                        return;
                    }
                }

                SaveOpenTimeSetting(timeId);
            }
            else if (id == PinColorSettingID.CloseTime)
            {
                if (timeId == PinColorSettingTimeID.Time2)
                {
                    if (CloseTimeDataDic[timeId].Time.Value <= TimeSpan.Parse("0:00"))
                    {
                        CloseTimeDataDic[timeId].Time.Value = TimeSpan.Parse("0:01");
                        return;
                    }
                    else if (CloseTimeDataDic[timeId + 1].Time.Value <= CloseTimeDataDic[timeId].Time.Value)
                    {
                        CloseTimeDataDic[timeId].Time.Value = CloseTimeDataDic[timeId + 1].Time.Value - TimeSpan.FromMinutes(1);
                        return;
                    }
                    CloseTimeFirstData.Time.Value = CloseTimeDataDic[timeId].Time.Value - TimeSpan.FromMinutes(1);
                    SaveCloseTimeSetting(PinColorSettingTimeID.Time1);
                }
                else if (timeId == PinColorSettingTimeID.Time7)
                {
                    if (CloseTimeDataDic[timeId].Time.Value <= CloseTimeDataDic[timeId - 1].Time.Value)
                    {
                        CloseTimeDataDic[timeId].Time.Value = CloseTimeDataDic[timeId - 1].Time.Value + TimeSpan.FromMinutes(1);
                        return;
                    }
                }
                else
                {
                    if (CloseTimeDataDic[timeId].Time.Value <= CloseTimeDataDic[timeId - 1].Time.Value)
                    {
                        CloseTimeDataDic[timeId].Time.Value = CloseTimeDataDic[timeId - 1].Time.Value + TimeSpan.FromMinutes(1);
                        return;
                    }
                    else if (CloseTimeDataDic[timeId + 1].Time.Value <= CloseTimeDataDic[timeId].Time.Value)
                    {
                        CloseTimeDataDic[timeId].Time.Value = CloseTimeDataDic[timeId + 1].Time.Value - TimeSpan.FromMinutes(1);
                        return;
                    }
                }

                SaveCloseTimeSetting(timeId);
            }
        }

        /// <summary>
        /// 開館時間閉館時間の有効状態変更時処理
        /// </summary>
        /// <param name="id">ピンカラー設定ID</param>
        /// <param name="timeId">ピンカラー設定タイムID</param>
        private void ChangeOpenCloseValid(PinColorSettingID id, PinColorSettingTimeID timeId)
        {
            if (_nowLoading == true) { return; }

            if (id == PinColorSettingID.OpenTime)
            {
                if (OpenTimeDataDic[timeId].IsValid.Value == true)
                {
                    if (timeId != PinColorSettingTimeID.Time6)
                    {
                        if (OpenTimeDataDic[timeId + 1].IsValid.Value == false)
                        {
                            OpenTimeDataDic[timeId].IsValid.Value = false;
                            return;
                        }
                    }
                }
                else
                {
                    if (timeId != PinColorSettingTimeID.Time1)
                    {
                        if (OpenTimeDataDic[timeId - 1].IsValid.Value == true)
                        {
                            OpenTimeDataDic[timeId].IsValid.Value = true;
                            return;
                        }
                    }
                }

                SaveOpenTimeSetting(timeId);
            }
            else if (id == PinColorSettingID.CloseTime)
            {
                if (CloseTimeDataDic[timeId].IsValid.Value == true)
                {
                    if (timeId != PinColorSettingTimeID.Time2)
                    {
                        if (CloseTimeDataDic[timeId - 1].IsValid.Value == false)
                        {
                            CloseTimeDataDic[timeId].IsValid.Value = false;
                            return;
                        }
                    }
                }
                else
                {
                    if (timeId != PinColorSettingTimeID.Time7)
                    {
                        if (CloseTimeDataDic[timeId + 1].IsValid.Value == true)
                        {
                            CloseTimeDataDic[timeId].IsValid.Value = true;
                            return;
                        }
                    }
                }

                SaveCloseTimeSetting(timeId);
            }
        }

        /// <summary>
        /// ピンカラー設定対象月変更時処理
        /// </summary>
        private void ChangePinColorMonth()
        {
            if (_nowLoading == true) { return; }

            _pinColorSettingDataModel.ChangePinColorMonth(PinColorMonth.Value);
        }

        /// <summary>
        /// ピンカラー設定 訪問状態 保存
        /// </summary>
        /// <param name="id">ID</param>
        private void SaveVisitedSetting(PinColorSettingVisitedID id)
        {
            _pinColorSettingDataModel.ChangeVisitedPinColorSetting(new PinColorSettingVisitedData(VisitedDataDic[id].ID, VisitedDataDic[id].Color.Value));
        }

        /// <summary>
        /// ピンカラー設定 開館時間 保存
        /// </summary>
        /// <param name="id">ID</param>
        private void SaveOpenTimeSetting(PinColorSettingTimeID id)
        {
            PinColorSettingOpenTimeData data;
            if (id == PinColorSettingTimeID.Time7)
            {
                data = new PinColorSettingOpenTimeData(OpenTimeLastData.ID, OpenTimeLastData.IsValid.Value, OpenTimeLastData.Time.Value, OpenTimeLastData.Color.Value);
            }
            else
            {
                data = new PinColorSettingOpenTimeData(OpenTimeDataDic[id].ID, OpenTimeDataDic[id].IsValid.Value, OpenTimeDataDic[id].Time.Value, OpenTimeDataDic[id].Color.Value);
            }
            _pinColorSettingDataModel.ChangeOpenTimePinColorSetting(data);
        }

        /// <summary>
        /// ピンカラー設定 閉館時間 保存
        /// </summary>
        /// <param name="id">ID</param>
        private void SaveCloseTimeSetting(PinColorSettingTimeID id)
        {
            PinColorSettingCloseTimeData data;
            if (id == PinColorSettingTimeID.Time1)
            {
                data = new PinColorSettingCloseTimeData(CloseTimeFirstData.ID, CloseTimeFirstData.IsValid.Value, CloseTimeFirstData.Time.Value, CloseTimeFirstData.Color.Value);
            }
            else
            {
                data = new PinColorSettingCloseTimeData(CloseTimeDataDic[id].ID, CloseTimeDataDic[id].IsValid.Value, CloseTimeDataDic[id].Time.Value, CloseTimeDataDic[id].Color.Value);
            }
            _pinColorSettingDataModel.ChangeCloseTimePinColorSetting(data);
        }

        /// <summary>
        /// ピンカラー設定 定休日 保存
        /// </summary>
        /// <param name="id">ID</param>
        private void SaveCloseDaySetting(PinColorSettingCloseDayID id)
        {
            _pinColorSettingDataModel.ChangeCloseDayPinColorSetting(new PinColorSettingCloseDayData(CloseDayDataDic[id].ID, CloseDayDataDic[id].Color.Value));
        }
    }
}
