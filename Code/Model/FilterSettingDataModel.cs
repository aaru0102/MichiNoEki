using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    /// <summary>
    /// フィルター設定データ管理Model
    /// </summary>
    public class FilterSettingDataModel
    {
        private bool _filterSettingLoaded = false;
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
        /// フィルターデータ
        /// </summary>
        private Dictionary<FilterSettingID, FilterSettingData> _filterSettingsDic = new Dictionary<FilterSettingID, FilterSettingData>();

        /// <summary>
        /// 道の駅フィルターデータ
        /// </summary>
        private Dictionary<int, FilterSettingDataModelFilterData> _michiNoEkiFilterDic = new Dictionary<int, FilterSettingDataModelFilterData>();

        /// <summary>
        /// 表示状態ロードイベント
        /// </summary>
        public EventHandler<AllUpdateVisibleEventArgs>? AllUpdateVisibleEvent { get; set; }

        /// <summary>
        /// 表示状態更新イベント
        /// </summary>
        public EventHandler<UpdateVisibleEventArgs>? UpdateVisibleEvent { get; set; }

        /// <summary>
        /// フィルター設定ロードイベント
        /// </summary>
        public EventHandler<LoadedFilterSettingInfoEventArgs>? LoadedFilterSettingInfoEvent { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FilterSettingDataModel()
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

            foreach (FilterSettingID item in Enum.GetValues(typeof(FilterSettingID)))
            {
                if (item == FilterSettingID.None) { continue; }
                _filterSettingsDic.Add(item, new FilterSettingData(item));
            }
        }

        /// <summary>
        /// フィルター更新
        /// </summary>
        /// <param name="data">フィルター設定データ</param>
        /// <returns></returns>
        public async Task<bool> UpdateFilterSetting(FilterSettingData data)
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

        /// <summary>
        /// データベースファイルロード完了時処理
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private async void OnDatabaseLoaded(object? sender, EventArgs e)
        {
            // フィルター有効状態を設定値から取得
            int filterValid = _app.GetSettingInfo(SettingID.Filter);
            if ((filterValid & (int)FilterSettingID.Opend) != 0)
            {
                _filterSettingsDic[FilterSettingID.Opend].IsValid = true;
            }
            if ((filterValid & (int)FilterSettingID.Visited) != 0)
            {
                _filterSettingsDic[FilterSettingID.Visited].IsValid = true;
            }
            if ((filterValid & (int)FilterSettingID.Region) != 0)
            {
                _filterSettingsDic[FilterSettingID.Region].IsValid = true;
            }

            // データベースから設定値取得
            List<FilterSettingInfo> tableList = await _app.DatabaseAccess.GetAllAsync<FilterSettingInfo>();

            // ディクショナリーにセット
            foreach (var item in tableList)
            {
                _filterSettingsDic[(FilterSettingID)item.ID].Value = item.Value;
            }

            // フィルター設定ロードイベント発火
            LoadedFilterSettingInfoEvent?.Invoke(this, new LoadedFilterSettingInfoEventArgs(_filterSettingsDic));

            // ロード済みとする
            _filterSettingLoaded = true;

            // 表示状態を更新
            if (_michiNoEkiInfoLoaded == true)
            {
                Dictionary<int, bool> dic = new Dictionary<int, bool>();
                foreach (var item in _michiNoEkiFilterDic.Keys)
                {
                    UpdateVisible(item);
                    dic.Add(item, _michiNoEkiFilterDic[item].IsVisible);
                }

                // 表示状態ロードイベント発火
                AllUpdateVisibleEvent?.Invoke(this, new AllUpdateVisibleEventArgs(dic));
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
                _michiNoEkiFilterDic.Add(item.ID, new FilterSettingDataModelFilterData(item.IsOpened, item.IsVisited, item.Region));
            }

            // ロード済みとする
            _michiNoEkiInfoLoaded = true;

            // 表示状態を更新
            if (_filterSettingLoaded == true)
            {
                Dictionary<int, bool> dic = new Dictionary<int, bool>();
                foreach (var item in _michiNoEkiFilterDic.Keys)
                {
                    UpdateVisible(item);
                    dic.Add(item, _michiNoEkiFilterDic[item].IsVisible);
                }

                // 表示状態ロードイベント発火
                AllUpdateVisibleEvent?.Invoke(this, new AllUpdateVisibleEventArgs(dic));
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
                    _michiNoEkiFilterDic[id].IsOpend = e.MichiNoEkiInfo.IsOpened;
                    _michiNoEkiFilterDic[id].IsVisited = e.MichiNoEkiInfo.IsVisited;
                    UpdateVisible(id);

                    // 表示状態更新イベント発火
                    UpdateVisibleEvent?.Invoke(this, new UpdateVisibleEventArgs(id, _michiNoEkiFilterDic[id].IsVisible));
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
        /// 道の駅の表示状態を更新する
        /// </summary>
        /// <param name="id">道の駅ID</param>
        private void UpdateVisible(int id)
        {
            // デフォルトは非表示
            bool isVisible = true;

            // オープン状態のチェック
            FilterSettingID filter = FilterSettingID.Opend;
            if (_filterSettingsDic[filter].IsValid == true)
            {
                bool openConditionMet = false;

                if ((_filterSettingsDic[filter].Value & (int)FilterSettingOpendID.NotOpened) != 0 && !_michiNoEkiFilterDic[id].IsOpend)
                {
                    openConditionMet = true;
                }

                if ((_filterSettingsDic[filter].Value & (int)FilterSettingOpendID.Opened) != 0 && _michiNoEkiFilterDic[id].IsOpend)
                {
                    openConditionMet = true;
                }

                if (!openConditionMet)
                {
                    isVisible = false;
                }
            }

            // 訪問状態のチェック
            filter = FilterSettingID.Visited;
            if (_filterSettingsDic[filter].IsValid == true)
            {
                bool visitedConditionMet = false;

                if ((_filterSettingsDic[filter].Value & (int)FilterSettingVisitedID.NotVisited) != 0 && !_michiNoEkiFilterDic[id].IsVisited)
                {
                    visitedConditionMet = true;
                }

                if ((_filterSettingsDic[filter].Value & (int)FilterSettingVisitedID.Visited) != 0 && _michiNoEkiFilterDic[id].IsVisited)
                {
                    visitedConditionMet = true;
                }

                if (!visitedConditionMet)
                {
                    isVisible = false;
                }
            }

            // 地方のチェック
            filter = FilterSettingID.Region;
            if (_filterSettingsDic[filter].IsValid == true)
            {
                bool regionConditionMet = false;

                if ((_filterSettingsDic[filter].Value & (int)FilterSettingRegionID.Hokkaido) != 0 && _michiNoEkiFilterDic[id].Region == "北海道地方")
                {
                    regionConditionMet = true;
                }
                else if ((_filterSettingsDic[filter].Value & (int)FilterSettingRegionID.Tohoku) != 0 && _michiNoEkiFilterDic[id].Region == "東北地方")
                {
                    regionConditionMet = true;
                }
                else if ((_filterSettingsDic[filter].Value & (int)FilterSettingRegionID.Kanto) != 0 && _michiNoEkiFilterDic[id].Region == "関東地方")
                {
                    regionConditionMet = true;
                }
                else if ((_filterSettingsDic[filter].Value & (int)FilterSettingRegionID.Hokuriku) != 0 && _michiNoEkiFilterDic[id].Region == "北陸地方")
                {
                    regionConditionMet = true;
                }
                else if ((_filterSettingsDic[filter].Value & (int)FilterSettingRegionID.Tyubu) != 0 && _michiNoEkiFilterDic[id].Region == "中部地方")
                {
                    regionConditionMet = true;
                }
                else if ((_filterSettingsDic[filter].Value & (int)FilterSettingRegionID.Kinki) != 0 && _michiNoEkiFilterDic[id].Region == "近畿地方")
                {
                    regionConditionMet = true;
                }
                else if ((_filterSettingsDic[filter].Value & (int)FilterSettingRegionID.Tyugoku) != 0 && _michiNoEkiFilterDic[id].Region == "中国地方")
                {
                    regionConditionMet = true;
                }
                else if ((_filterSettingsDic[filter].Value & (int)FilterSettingRegionID.Shikoku) != 0 && _michiNoEkiFilterDic[id].Region == "四国地方")
                {
                    regionConditionMet = true;
                }
                else if ((_filterSettingsDic[filter].Value & (int)FilterSettingRegionID.Kyusyu) != 0 && _michiNoEkiFilterDic[id].Region == "九州地方")
                {
                    regionConditionMet = true;
                }

                if (!regionConditionMet)
                {
                    isVisible = false;
                }
            }

            // 結果を設定
            _michiNoEkiFilterDic[id].IsVisible = isVisible;
        }
    }
}
