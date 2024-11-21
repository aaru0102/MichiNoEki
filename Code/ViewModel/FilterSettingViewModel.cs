using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    public class FilterSettingViewModel
    {
        private bool _loading = false;

        /// <summary>
        /// フィルター設定データ管理Model
        /// </summary>
        private FilterSettingDataModel _filterSettingDataModel;

        /// <summary>
        /// フィルター設定リスト
        /// </summary>
        public Dictionary<FilterSettingID, FilterSettingViewFilterListItem> FilterList { get; set; } = new Dictionary<FilterSettingID, FilterSettingViewFilterListItem>()
        {
            { FilterSettingID.Opend, new FilterSettingViewFilterListItem(FilterSettingID.Opend) },
            { FilterSettingID.Visited, new FilterSettingViewFilterListItem(FilterSettingID.Visited) },
            { FilterSettingID.Region, new FilterSettingViewFilterListItem(FilterSettingID.Region) },
        };

        /// <summary>
        /// 詳細フィルター表示コマンド
        /// </summary>
        public DelegateCommand OpenDetailFilterCommand { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FilterSettingViewModel()
        {
            // フィルター設定データ管理Model取得
            _filterSettingDataModel = App.Instance.FilterSettingDataModel;

            // フィルター設定データ管理Modelのイベント捕捉
            _filterSettingDataModel.LoadedFilterSettingInfoEvent += OnLoadedFilterSettingInfoEvent;

            // コマンドハンドラ登録
            OpenDetailFilterCommand = new DelegateCommand(OnOpenDetailFilterCommand);

            // チェック状態が切り替わったときにフィルター設定を更新する
            foreach (var parentID in FilterList.Keys)
            {
                FilterList[parentID].IsChecked.PropertyChanged += (s, e) =>
                {
                    if (_loading == false)
                    {
                        UpdateFilter(parentID);
                    }
                };

                foreach (var id in FilterList[parentID].DetailDic.Keys)
                {
                    FilterList[parentID].DetailDic[id].IsChecked.PropertyChanged += (s, e) =>
                    {
                        if (_loading == false)
                        {
                            UpdateFilter(parentID);
                        }
                    };
                }
            }
        }

        /// <summary>
        /// フィルター設定ロードイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void OnLoadedFilterSettingInfoEvent(object? sender, LoadedFilterSettingInfoEventArgs e)
        {
            _loading = true;

            // 設定情報を反映
            foreach (var item in e.FilterSettingsDic)
            {
                FilterList[item.Key].IsChecked.Value = item.Value.IsValid;

                switch (item.Key)
                {
                    case FilterSettingID.Opend:
                        foreach (FilterSettingOpendID item1 in Enum.GetValues(typeof(FilterSettingOpendID)))
                        {
                            if (item1 == FilterSettingOpendID.None) { continue; }
                            int id = (int)item1;
                            FilterList[item.Key].DetailDic[id].IsChecked.Value = (item.Value.Value & id) != 0;
                        }
                        break;

                    case FilterSettingID.Visited:
                        foreach (FilterSettingVisitedID item1 in Enum.GetValues(typeof(FilterSettingVisitedID)))
                        {
                            if (item1 == FilterSettingVisitedID.None) { continue; }
                            int id = (int)item1;
                            FilterList[item.Key].DetailDic[id].IsChecked.Value = (item.Value.Value & id) != 0;
                        }
                        break;

                    case FilterSettingID.Region:
                        foreach (FilterSettingRegionID item1 in Enum.GetValues(typeof(FilterSettingRegionID)))
                        {
                            if (item1 == FilterSettingRegionID.None) { continue; }
                            int id = (int)item1;
                            FilterList[item.Key].DetailDic[id].IsChecked.Value = (item.Value.Value & id) != 0;
                        }
                        break;

                    default:
                        break;
                }
            }

            _loading = false;
        }

        /// <summary>
        /// 詳細フィルター表示コマンドハンドラ
        /// </summary>
        /// <param name="param">コマンド引数</param>
        private void OnOpenDetailFilterCommand(object? param)
        {
            if (param is FilterSettingID id)
            {
                // 詳細フィルター表示状態を切り替える
                FilterList[id].IsOpened.Value = !FilterList[id].IsOpened.Value;
            }
        }

        /// <summary>
        /// フィルター設定更新
        /// </summary>
        /// <param name="param">コマンド引数</param>
        private void UpdateFilter(FilterSettingID id)
        {
            int value = 0;
            foreach (var item in FilterList[id].DetailDic)
            {
                if (item.Value.IsChecked.Value == true)
                {
                    value |= item.Key;
                }
            }

            _ = _filterSettingDataModel.UpdateFilterSetting(new FilterSettingData(id) { IsValid = FilterList[id].IsChecked.Value, Value = value });
        }
    }
}
