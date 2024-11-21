using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace RoadsideStationApp
{
    public class PinColorSettingOpenTimeInfo
    {
        /// <summary>
        /// ID(PinColorSettingVisitedID)
        /// </summary>
        [PrimaryKey]
        public int ID { get; set; }

        /// <summary>
        /// 有効状態
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 時間(HH:mm)
        /// </summary>
        public string Time { get; set; } = "23:59";

        /// <summary>
        /// 色(#XXXXXXXX)
        /// </summary>
        public string Color { get; set; } = "#FFFFFFFF";
    }
}
