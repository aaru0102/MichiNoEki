using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// フィルター更新
        /// </summary>
        /// <param name="id">ピンカラー設定切り替え</param>
        /// <returns></returns>
        public async Task<bool> ChangePinColorSetting(PinColorSettingID id)
        {
            // データベースに反映
            bool result = await _app.DatabaseAccess.UpdateAsync(new FilterSettingInfo() { ID = (int)data.ID, Value = data.Value });
            if (_filterSettingsDic[data.ID].IsValid != data.IsValid)
            {
                int filterValid = 0;
                foreach (FilterSettingID item in Enum.GetValues(typeof(FilterSettingID)))
                {
                    if (item == FilterSettingID.None) { continue; }
                    if (item == data.ID)
                    {
                        if (data.IsValid == true)
                        {
                            filterValid |= (int)item;
                        }
                    }
                    else if (_filterSettingsDic[item].IsValid == true)
                    {
                        filterValid |= (int)item;
                    }
                }
                result &= await _app.DatabaseAccess.UpdateAsync(new SettingInfo() { ID = (int)SettingID.Filter, Value = filterValid });
            }

            if (result == true)
            {
                // フィルター設定データ更新
                _filterSettingsDic[data.ID].Value = data.Value;
                _filterSettingsDic[data.ID].IsValid = data.IsValid;

                // 表示状態更新
                Dictionary<int, bool> dic = new Dictionary<int, bool>();
                foreach (var item in _michiNoEkiFilterDic.Keys)
                {
                    UpdateVisible(item);
                    dic.Add(item, _michiNoEkiFilterDic[item].IsVisible);
                }

                // 表示状態ロードイベント発火
                AllUpdateVisibleEvent?.Invoke(this, new AllUpdateVisibleEventArgs(dic));
            }

            return result;
        }
    }
}
