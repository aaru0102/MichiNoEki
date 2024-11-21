using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    /// <summary>
    /// フィルター設定データ
    /// </summary>
    public class FilterSettingData
    {
        /// <summary>
        /// ID
        /// </summary>
        public FilterSettingID ID { get; set; }

        /// <summary>
        /// 有効
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// フィルター値
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        public FilterSettingData(FilterSettingID id)
        {
            ID = id;
            IsValid = false;
            Value = 0;
        }
    }
}
