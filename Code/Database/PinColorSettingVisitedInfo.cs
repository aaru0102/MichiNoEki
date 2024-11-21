using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace RoadsideStationApp
{
    public class PinColorSettingVisitedInfo
    {
        /// <summary>
        /// ID(PinColorSettingVisitedID)
        /// </summary>
        [PrimaryKey]
        public int ID { get; set; }

        /// <summary>
        /// 色(#XXXXXXXX)
        /// </summary>
        public string Color { get; set; } = "#FFFFFFFF";
    }
}
