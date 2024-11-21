using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    public enum PinColorSettingID
    {
        /// <summary>
        /// 訪問状態
        /// </summary>
        Visited = 1,

        /// <summary>
        /// 開館時間
        /// </summary>
        OpenTime = 2,

        /// <summary>
        /// 閉館時間
        /// </summary>
        CloseTime = 3,

        /// <summary>
        /// 定休日
        /// </summary>
        CloseDay = 4,
    }
}
