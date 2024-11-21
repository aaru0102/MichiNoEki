using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace RoadsideStationApp
{
    public class FilterSettingInfo
    {
        /// <summary>
        /// ID(FilterSettingID)
        /// </summary>
        [PrimaryKey]
        public int ID { get; set; }

        /// <summary>
        /// 設定値
        /// </summary>
        public int Value { get; set; }
    }
}
