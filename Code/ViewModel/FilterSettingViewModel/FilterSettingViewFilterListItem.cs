using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    public class FilterSettingViewFilterListItem
    {
        /// <summary>
        /// ID
        /// </summary>
        public FilterSettingID ID { get; set; }

        /// <summary>
        /// フィルター名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// チェック状態
        /// </summary>
        public AutoNotifyProperty<bool> IsChecked { get; set; } = new AutoNotifyProperty<bool>();

        /// <summary>
        /// 詳細オープン状態
        /// </summary>
        public AutoNotifyProperty<bool> IsOpened { get; set; } = new AutoNotifyProperty<bool>();

        /// <summary>
        /// 詳細リスト
        /// </summary>
        public Dictionary<int, FilterSettingViewFilterDetailListItem> DetailDic { get; set; } = new Dictionary<int, FilterSettingViewFilterDetailListItem>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">フィルターID</param>
        public FilterSettingViewFilterListItem(FilterSettingID id)
        {
            ID = id;
            IsChecked.Value = true;
            IsOpened.Value = false;

            int detailID;
            switch(id)
            {
                case FilterSettingID.Opend:
                    Name = "オープン状態";
                    detailID = (int)FilterSettingOpendID.NotOpened;
                    DetailDic.Add(detailID, new FilterSettingViewFilterDetailListItem(id, detailID, "未オープン"));
                    detailID = (int)FilterSettingOpendID.Opened;
                    DetailDic.Add(detailID, new FilterSettingViewFilterDetailListItem(id, detailID, "オープン済み"));
                    break;

                case FilterSettingID.Visited:
                    Name = "訪問状態";
                    detailID = (int)FilterSettingVisitedID.NotVisited;
                    DetailDic.Add(detailID, new FilterSettingViewFilterDetailListItem(id, detailID, "未訪問"));
                    detailID = (int)FilterSettingVisitedID.Visited;
                    DetailDic.Add(detailID, new FilterSettingViewFilterDetailListItem(id, detailID, "訪問済み"));
                    break;

                case FilterSettingID.Region:
                    Name = "地方";
                    detailID = (int)FilterSettingRegionID.Hokkaido;
                    DetailDic.Add(detailID, new FilterSettingViewFilterDetailListItem(id, detailID, "北海道地方"));
                    detailID = (int)FilterSettingRegionID.Tohoku;
                    DetailDic.Add(detailID, new FilterSettingViewFilterDetailListItem(id, detailID, "東北地方"));
                    detailID = (int)FilterSettingRegionID.Kanto;
                    DetailDic.Add(detailID, new FilterSettingViewFilterDetailListItem(id, detailID, "関東地方"));
                    detailID = (int)FilterSettingRegionID.Hokuriku;
                    DetailDic.Add(detailID, new FilterSettingViewFilterDetailListItem(id, detailID, "北陸地方"));
                    detailID = (int)FilterSettingRegionID.Tyubu;
                    DetailDic.Add(detailID, new FilterSettingViewFilterDetailListItem(id, detailID, "中部地方"));
                    detailID = (int)FilterSettingRegionID.Kinki;
                    DetailDic.Add(detailID, new FilterSettingViewFilterDetailListItem(id, detailID, "近畿地方"));
                    detailID = (int)FilterSettingRegionID.Tyugoku;
                    DetailDic.Add(detailID, new FilterSettingViewFilterDetailListItem(id, detailID, "中国地方"));
                    detailID = (int)FilterSettingRegionID.Shikoku;
                    DetailDic.Add(detailID, new FilterSettingViewFilterDetailListItem(id, detailID, "四国地方"));
                    detailID = (int)FilterSettingRegionID.Kyusyu;
                    DetailDic.Add(detailID, new FilterSettingViewFilterDetailListItem(id, detailID, "九州地方"));
                    break;

                default:
                    break;
            }
        }
    }
}
