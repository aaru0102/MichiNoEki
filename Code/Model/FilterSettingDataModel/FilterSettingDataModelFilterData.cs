using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    public class FilterSettingDataModelFilterData
    {
        /// <summary>
        /// 表示状態
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// オープン済み
        /// </summary>
        public bool IsOpend { get; set; }

        /// <summary>
        /// 訪問済み
        /// </summary>
        public bool IsVisited { get; set; }

        /// <summary>
        /// 地方名称
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="isOpend">オープン済み</param>
        /// <param name="isVisted">訪問済み</param>
        /// <param name="region">地方名称</param>
        public FilterSettingDataModelFilterData(bool isOpend, bool isVisted, string region)
        {
            IsVisible = true;
            IsOpend = isOpend;
            IsVisited = isVisted;
            Region = region;
        }
    }
}
