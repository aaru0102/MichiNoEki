using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    public enum FilterSettingID
    {
        /// <summary>
        /// 設定なし
        /// </summary>
        None = 0x00,

        /// <summary>
        /// オープン状態
        /// </summary>
        Opend = 0x01,

        /// <summary>
        /// 訪問状態
        /// </summary>
        Visited = 0x02,

        /// <summary>
        /// 地方
        /// </summary>
        Region = 0x04,
    }
}
