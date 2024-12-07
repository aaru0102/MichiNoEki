using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CoreFoundation.DispatchSource;

namespace RoadsideStationApp
{
    public class PinColorSettingDataModel
    {
        private bool _pinColorSettingLoaded = false;
        private bool _michiNoEkiInfoLoaded = false;

        /// <summary>
        /// Appインスタンス
        /// </summary>
        private App _app;

        /// <summary>
        /// 道の駅データ管理Model
        /// </summary>
        private MichiNoEkiDataModel _michiNoEkiDataModel;

        /// <summary>
        /// 有効なピンカラー設定
        /// </summary>
        private PinColorSettingID _validPinColorSetting;
        
        /// <summary>
        /// ピンカラー設定対象月
        /// </summary>
        private int _pinColorMonth;

        /// <summary>
        /// 未オープン ピンカラー
        /// </summary>
        private Color _notOpendPinColor = new Color();

        /// <summary>
        /// スタンプ24時間押下OK ピンカラー
        /// </summary>
        private Color _stampAllTimeOKPinColor = new Color();

        /// <summary>
        /// ピンカラー設定 訪問状態 設定内容
        /// </summary>
        private Dictionary<PinColorSettingVisitedID, PinColorSettingVisitedData> _visitedDataDic = new Dictionary<PinColorSettingVisitedID, PinColorSettingVisitedData>();

        /// <summary>
        /// ピンカラー設定 開館時間 設定内容
        /// </summary>
        private Dictionary<PinColorSettingTimeID, PinColorSettingOpenTimeData> _openTimeDataDic = new Dictionary<PinColorSettingTimeID, PinColorSettingOpenTimeData>();

        /// <summary>
        /// ピンカラー設定 閉館時間 設定内容
        /// </summary>
        private Dictionary<PinColorSettingTimeID, PinColorSettingCloseTimeData> _closeTimeDataDic = new Dictionary<PinColorSettingTimeID, PinColorSettingCloseTimeData>();

        /// <summary>
        /// ピンカラー設定 定休日 設定内容
        /// </summary>
        private Dictionary<PinColorSettingCloseDayID, PinColorSettingCloseDayData> _closeDayDataDic = new Dictionary<PinColorSettingCloseDayID, PinColorSettingCloseDayData>();

        /// <summary>
        /// ピンカラーデータ
        /// </summary>
        private Dictionary<int, Color> _michiNoEkiPinColorDic = new Dictionary<int, Color>();

        /// <summary>
        /// 道の駅データ
        /// </summary>
        private Dictionary<int, MichiNoEkiInfo> _michiNoEkiInfoDic = new Dictionary<int, MichiNoEkiInfo>();

        /// <summary>
        /// ピンカラー全件更新イベント
        /// </summary>
        public EventHandler<AllUpdatePinColorEventArgs>? AllUpdatePinColorEvent { get; set; }

        /// <summary>
        /// ピンカラー更新イベント
        /// </summary>
        public EventHandler<UpdatePinColorEventArgs>? UpdatePinColorEvent { get; set; }

        /// <summary>
        /// ピンカラー設定ロードイベント
        /// </summary>
        public EventHandler<LoadedPinColorSettingInfoEventArgs>? LoadedPinColorSettingInfoEvent { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PinColorSettingDataModel()
        {
            // Appのインスタンス取得
            _app = App.Instance;

            // データベースファイルロード完了時処理を登録
            _app.DatabaseLoaded += OnDatabaseLoaded;

            // 道の駅データ管理Modelのインスタンス取得
            _michiNoEkiDataModel = App.Instance.MichiNoEkiDataModel;

            // 道の駅データ関連のイベント捕捉
            _michiNoEkiDataModel.LoadedMichiNoEkiInfoEvent += OnLoadedMichiNoEkiInfoEvent;
            _michiNoEkiDataModel.UpdateMichiNoEkiInfoEvent += OnUpdateMichiNoEkiInfoEvent;
        }

        /// <summary>
        /// 有効ピンカラー設定変更
        /// </summary>
        /// <param name="id">ピンカラー設定ID</param>
        /// <returns></returns>
        public async void ChangePinColorSetting(PinColorSettingID id)
        {
            // データベースに反映
            await _app.DatabaseAccess.UpdateAsync(new SettingInfo() { ID = (int)SettingID.PinColor, Value = (int)id });

            // 有効ピンカラー設定データ更新
            _validPinColorSetting = id;

            // 表示状態更新
            Dictionary<int, Color> dic = new Dictionary<int, Color>();
            foreach (var item in _michiNoEkiPinColorDic.Keys)
            {
                UpdatePinColor(item);
                dic.Add(item, _michiNoEkiPinColorDic[item]);
            }

            // ピンカラー全件更新イベント発火
            AllUpdatePinColorEvent?.Invoke(this, new AllUpdatePinColorEventArgs(dic));
        }

        /// <summary>
        /// 未オープンピンカラー変更
        /// </summary>
        /// <param name="color">ピンカラー</param>
        /// <returns></returns>
        public async void ChangeNotOpendPinColorSetting(Color color)
        {
            // データベースに反映
            await _app.DatabaseAccess.UpdateAsync(new SettingInfo() { ID = (int)SettingID.NotOpendPinColor, Value = color.ToInt() });

            // 未オープンピンカラー更新
            _notOpendPinColor = color;

            // 表示状態更新
            Dictionary<int, Color> dic = new Dictionary<int, Color>();
            foreach (var item in _michiNoEkiPinColorDic.Keys)
            {
                UpdatePinColor(item);
                dic.Add(item, _michiNoEkiPinColorDic[item]);
            }

            // ピンカラー全件更新イベント発火
            AllUpdatePinColorEvent?.Invoke(this, new AllUpdatePinColorEventArgs(dic));
        }

        /// <summary>
        /// スタンプ24時間押下OKピンカラー変更
        /// </summary>
        /// <param name="color">ピンカラー</param>
        /// <returns></returns>
        public async void ChangeStampAllTimeOKPinColorSetting(Color color)
        {
            // データベースに反映
            await _app.DatabaseAccess.UpdateAsync(new SettingInfo() { ID = (int)SettingID.StampAllTimeOKPinColor, Value = color.ToInt() });

            // スタンプ24時間押下OKピンカラー更新
            _stampAllTimeOKPinColor = color;

            // 表示状態更新
            Dictionary<int, Color> dic = new Dictionary<int, Color>();
            foreach (var item in _michiNoEkiPinColorDic.Keys)
            {
                UpdatePinColor(item);
                dic.Add(item, _michiNoEkiPinColorDic[item]);
            }

            // ピンカラー全件更新イベント発火
            AllUpdatePinColorEvent?.Invoke(this, new AllUpdatePinColorEventArgs(dic));
        }

        /// <summary>
        /// ピンカラー設定対象月変更
        /// </summary>
        /// <param name="month">月</param>
        /// <returns></returns>
        public async void ChangePinColorMonth(int month)
        {
            // データベースに反映
            await _app.DatabaseAccess.UpdateAsync(new SettingInfo() { ID = (int)SettingID.PinColorMonth, Value = month });

            // ピンカラー設定対象月更新
            _pinColorMonth = month;

            // 表示状態更新
            Dictionary<int, Color> dic = new Dictionary<int, Color>();
            foreach (var item in _michiNoEkiPinColorDic.Keys)
            {
                UpdatePinColor(item);
                dic.Add(item, _michiNoEkiPinColorDic[item]);
            }

            // ピンカラー全件更新イベント発火
            AllUpdatePinColorEvent?.Invoke(this, new AllUpdatePinColorEventArgs(dic));
        }

        /// <summary>
        /// 訪問状態ピンカラー設定変更
        /// </summary>
        /// <param name="data">データ</param>
        /// <returns></returns>
        public async void ChangeVisitedPinColorSetting(PinColorSettingVisitedData data)
        {
            // データベースに反映
            await _app.DatabaseAccess.UpdateAsync(new PinColorSettingVisitedInfo() { ID = (int)data.ID, Color = data.Color.ToArgbHex() });

            // ピンカラー設定データ更新
            _visitedDataDic[data.ID].Color = data.Color;

            // 表示状態更新
            Dictionary<int, Color> dic = new Dictionary<int, Color>();
            foreach (var item in _michiNoEkiPinColorDic.Keys)
            {
                UpdatePinColor(item);
                dic.Add(item, _michiNoEkiPinColorDic[item]);
            }

            // ピンカラー全件更新イベント発火
            AllUpdatePinColorEvent?.Invoke(this, new AllUpdatePinColorEventArgs(dic));
        }

        /// <summary>
        /// 開館時間ピンカラー設定変更
        /// </summary>
        /// <param name="data">データ</param>
        /// <returns></returns>
        public async void ChangeOpenTimePinColorSetting(PinColorSettingOpenTimeData data)
        {
            // データベースに反映
            await _app.DatabaseAccess.UpdateAsync(new PinColorSettingOpenTimeInfo() { ID = (int)data.ID, IsValid = data.IsValid, Time = data.Time.ToString(@"hh':'mm"), Color = data.Color.ToArgbHex() });

            // ピンカラー設定データ更新
            _openTimeDataDic[data.ID].IsValid = data.IsValid;
            _openTimeDataDic[data.ID].Time = data.Time;
            _openTimeDataDic[data.ID].Color = data.Color;

            // 表示状態更新
            Dictionary<int, Color> dic = new Dictionary<int, Color>();
            foreach (var item in _michiNoEkiPinColorDic.Keys)
            {
                UpdatePinColor(item);
                dic.Add(item, _michiNoEkiPinColorDic[item]);
            }

            // ピンカラー全件更新イベント発火
            AllUpdatePinColorEvent?.Invoke(this, new AllUpdatePinColorEventArgs(dic));
        }

        /// <summary>
        /// 閉館時間ピンカラー設定変更
        /// </summary>
        /// <param name="data">データ</param>
        /// <returns></returns>
        public async void ChangeCloseTimePinColorSetting(PinColorSettingCloseTimeData data)
        {
            // データベースに反映
            await _app.DatabaseAccess.UpdateAsync(new PinColorSettingCloseTimeInfo() { ID = (int)data.ID, IsValid = data.IsValid, Time = data.Time.ToString(@"hh':'mm"), Color = data.Color.ToArgbHex() });

            // ピンカラー設定データ更新
            _closeTimeDataDic[data.ID].IsValid = data.IsValid;
            _closeTimeDataDic[data.ID].Time = data.Time;
            _closeTimeDataDic[data.ID].Color = data.Color;

            // 表示状態更新
            Dictionary<int, Color> dic = new Dictionary<int, Color>();
            foreach (var item in _michiNoEkiPinColorDic.Keys)
            {
                UpdatePinColor(item);
                dic.Add(item, _michiNoEkiPinColorDic[item]);
            }

            // ピンカラー全件更新イベント発火
            AllUpdatePinColorEvent?.Invoke(this, new AllUpdatePinColorEventArgs(dic));
        }

        /// <summary>
        /// 定休日ピンカラー設定変更
        /// </summary>
        /// <param name="data">データ</param>
        /// <returns></returns>
        public async void ChangeCloseDayPinColorSetting(PinColorSettingCloseDayData data)
        {
            // データベースに反映
            await _app.DatabaseAccess.UpdateAsync(new PinColorSettingCloseDayInfo() { ID = (int)data.ID, Color = data.Color.ToArgbHex() });

            // ピンカラー設定データ更新
            _closeDayDataDic[data.ID].Color = data.Color;

            // 表示状態更新
            Dictionary<int, Color> dic = new Dictionary<int, Color>();
            foreach (var item in _michiNoEkiPinColorDic.Keys)
            {
                UpdatePinColor(item);
                dic.Add(item, _michiNoEkiPinColorDic[item]);
            }

            // ピンカラー全件更新イベント発火
            AllUpdatePinColorEvent?.Invoke(this, new AllUpdatePinColorEventArgs(dic));
        }

        /// <summary>
        /// データベースファイルロード完了時処理
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private async void OnDatabaseLoaded(object? sender, EventArgs e)
        {
            // 有効なピンカラー設定を設定値から取得
            _validPinColorSetting = (PinColorSettingID)_app.GetSettingInfo(SettingID.PinColor);

            // ピンカラー設定対象月を設定値から取得
            _pinColorMonth = _app.GetSettingInfo(SettingID.PinColorMonth);

            // 未オープンピンカラーを設定値から取得
            _notOpendPinColor = Color.FromInt(_app.GetSettingInfo(SettingID.NotOpendPinColor));

            // スタンプ24時間押下OKピンカラーを設定値から取得
            _stampAllTimeOKPinColor = Color.FromInt(_app.GetSettingInfo(SettingID.StampAllTimeOKPinColor));

            // データベースから各ピンカラー設定値取得
            List<PinColorSettingVisitedInfo> visitedInfoList = await _app.DatabaseAccess.GetAllAsync<PinColorSettingVisitedInfo>();
            List<PinColorSettingOpenTimeInfo> openTimeInfoList = await _app.DatabaseAccess.GetAllAsync<PinColorSettingOpenTimeInfo>();
            List<PinColorSettingCloseTimeInfo> closeTimeInfoList = await _app.DatabaseAccess.GetAllAsync<PinColorSettingCloseTimeInfo>();
            List<PinColorSettingCloseDayInfo> closeDayInfoList = await _app.DatabaseAccess.GetAllAsync<PinColorSettingCloseDayInfo>();

            // ディクショナリーにセット
            foreach (var item in visitedInfoList)
            {
                _visitedDataDic.Add((PinColorSettingVisitedID)item.ID, new PinColorSettingVisitedData(item));
            }
            foreach (var item in openTimeInfoList)
            {
                _openTimeDataDic.Add((PinColorSettingTimeID)item.ID, new PinColorSettingOpenTimeData(item));
            }
            foreach (var item in closeTimeInfoList)
            {
                _closeTimeDataDic.Add((PinColorSettingTimeID)item.ID, new PinColorSettingCloseTimeData(item));
            }
            foreach (var item in closeDayInfoList)
            {
                _closeDayDataDic.Add((PinColorSettingCloseDayID)item.ID, new PinColorSettingCloseDayData(item));
            }

            // フィルター設定ロードイベント発火
            LoadedPinColorSettingInfoEvent?.Invoke(this, new LoadedPinColorSettingInfoEventArgs(_validPinColorSetting, _pinColorMonth, _notOpendPinColor, _stampAllTimeOKPinColor, _visitedDataDic, _openTimeDataDic, _closeTimeDataDic, _closeDayDataDic));

            // ロード済みとする
            _pinColorSettingLoaded = true;

            // 表示状態を更新
            if (_michiNoEkiInfoLoaded == true)
            {
                Dictionary<int, Color> dic = new Dictionary<int, Color>();
                foreach (var item in _michiNoEkiPinColorDic.Keys)
                {
                    UpdatePinColor(item);
                    dic.Add(item, _michiNoEkiPinColorDic[item]);
                }

                // ピンカラー全件更新イベント発火
                AllUpdatePinColorEvent?.Invoke(this, new AllUpdatePinColorEventArgs(dic));
            }
        }

        /// <summary>
        /// 道の駅データロード完了イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void OnLoadedMichiNoEkiInfoEvent(object? sender, LoadedMichiNoEkiInfoEventArgs e)
        {
            // 道の駅データ初回セット
            foreach (var item in e.MichiNoEkiInfoList)
            {
                _michiNoEkiInfoDic.Add(item.ID, item);
            }

            // ロード済みとする
            _michiNoEkiInfoLoaded = true;

            // 表示状態を更新
            if (_pinColorSettingLoaded == true)
            {
                Dictionary<int, Color> dic = new Dictionary<int, Color>();
                foreach (var item in _michiNoEkiInfoDic.Keys)
                {
                    UpdatePinColor(item);
                    dic.Add(item, _michiNoEkiPinColorDic[item]);
                }

                // ピンカラー全件更新イベント発火
                AllUpdatePinColorEvent?.Invoke(this, new AllUpdatePinColorEventArgs(dic));
            }
        }

        /// <summary>
        /// 道の駅データ更新イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void OnUpdateMichiNoEkiInfoEvent(object? sender, UpdateMichiNoEkiInfoEventArgs e)
        {
            switch (e.Kind)
            {
                // 更新
                case UpdateKind.Update:
                    // 表示状態更新
                    int id = e.MichiNoEkiInfo.ID;
                    _michiNoEkiInfoDic[id] = e.MichiNoEkiInfo;
                    UpdatePinColor(id);

                    // 表示状態更新イベント発火
                    UpdatePinColorEvent?.Invoke(this, new UpdatePinColorEventArgs(id, _michiNoEkiPinColorDic[id]));
                    break;

                // 追加(機能なし)
                case UpdateKind.Add:
                    break;

                // 削除(機能なし)
                case UpdateKind.Delete:
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 道の駅のピンカラーを更新する
        /// </summary>
        /// <param name="id">道の駅ID</param>
        private void UpdatePinColor(int id)
        {
            switch (_validPinColorSetting)
            {
                case PinColorSettingID.Visited:
                    // 未オープンの場合は未オープンの色を採用
                    if (_michiNoEkiInfoDic[id].IsOpened == false)
                    {
                        _michiNoEkiPinColorDic[id] = _notOpendPinColor;
                        break;
                    }

                    // 訪問状態でピンカラー決定
                    if (_michiNoEkiInfoDic[id].IsVisited == true)
                    {
                        _michiNoEkiPinColorDic[id] = _visitedDataDic[PinColorSettingVisitedID.Visited].Color;
                    }
                    else
                    {
                        _michiNoEkiPinColorDic[id] = _visitedDataDic[PinColorSettingVisitedID.NotVisited].Color;
                    }

                    break;

                case PinColorSettingID.OpenTime:
                    // 未オープンの場合は未オープンの色を採用
                    if (_michiNoEkiInfoDic[id].IsOpened == false)
                    {
                        _michiNoEkiPinColorDic[id] = _notOpendPinColor;
                        break;
                    }

                    // スタンプ24時間押下OKの場合はスタンプ24時間押下OKの色を採用
                    if (_michiNoEkiInfoDic[id].StampAllTimeOK == true)
                    {
                        _michiNoEkiPinColorDic[id] = _stampAllTimeOKPinColor;
                        break;
                    }

                    // 開館時間でピンカラー決定
                    foreach (var item in _openTimeDataDic)
                    {
                        if (item.Value.ID == PinColorSettingTimeID.Time7)
                        {
                            _michiNoEkiPinColorDic[id] = _openTimeDataDic[item.Value.ID].Color;
                            break;
                        }

                        if (_openTimeDataDic[item.Value.ID].IsValid == true)
                        {
                            if (_michiNoEkiInfoDic[id].OpenTimeList[_pinColorMonth - 1] <= _openTimeDataDic[item.Value.ID].Time)
                            {
                                _michiNoEkiPinColorDic[id] = _openTimeDataDic[item.Value.ID].Color;
                                break;
                            }
                        }
                    }

                    break;

                case PinColorSettingID.CloseTime:
                    // 未オープンの場合は未オープンの色を採用
                    if (_michiNoEkiInfoDic[id].IsOpened == false)
                    {
                        _michiNoEkiPinColorDic[id] = _notOpendPinColor;
                        break;
                    }

                    // スタンプ24時間押下OKの場合はスタンプ24時間押下OKの色を採用
                    if (_michiNoEkiInfoDic[id].StampAllTimeOK == true)
                    {
                        _michiNoEkiPinColorDic[id] = _stampAllTimeOKPinColor;
                        break;
                    }

                    // 閉館時間でピンカラー決定
                    foreach (var item in _closeTimeDataDic.Reverse())
                    {
                        if (item.Value.ID == PinColorSettingTimeID.Time1)
                        {
                            _michiNoEkiPinColorDic[id] = _closeTimeDataDic[item.Value.ID].Color;
                            break;
                        }

                        if (_closeTimeDataDic[item.Value.ID].IsValid == true)
                        {
                            if (_michiNoEkiInfoDic[id].CloseTimeList[_pinColorMonth - 1] >= _closeTimeDataDic[item.Value.ID].Time)
                            {
                                _michiNoEkiPinColorDic[id] = _closeTimeDataDic[item.Value.ID].Color;
                                break;
                            }
                        }
                    }

                    break;

                case PinColorSettingID.CloseDay:
                    // 未オープンの場合は未オープンの色を採用
                    if (_michiNoEkiInfoDic[id].IsOpened == false)
                    {
                        _michiNoEkiPinColorDic[id] = _notOpendPinColor;
                        break;
                    }

                    // スタンプ24時間押下OKの場合はスタンプ24時間押下OKの色を採用
                    if (_michiNoEkiInfoDic[id].StampAllTimeOK == true)
                    {
                        _michiNoEkiPinColorDic[id] = _stampAllTimeOKPinColor;
                        break;
                    }

                    // 定休日でピンカラー決定
                    foreach (var item in _closeDayDataDic)
                    {
                        if (item.Value.ID == PinColorSettingCloseDayID.Nothing)
                        {
                            _michiNoEkiPinColorDic[id] = _closeDayDataDic[item.Value.ID].Color;
                            break;
                        }
                        else
                        {
                            if (_michiNoEkiInfoDic[id].CloseDayList[(int)item.Value.ID - 1] == true)
                            {
                                _michiNoEkiPinColorDic[id] = _closeDayDataDic[item.Value.ID].Color;
                                break;
                            }
                        }
                    }

                    break;

                default:
                    break;
            }
        }
    }
}
