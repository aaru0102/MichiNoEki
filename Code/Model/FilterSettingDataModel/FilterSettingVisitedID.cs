using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    public enum FilterSettingVisitedID
    {
        /// <summary>
        /// 設定なし
        /// </summary>
        None = 0x00,

        /// <summary>
        /// 未訪問
        /// </summary>
        NotVisited = 0x01,

        /// <summary>
        /// 訪問済み
        /// </summary>
        Visited = 0x02,
    }
}
