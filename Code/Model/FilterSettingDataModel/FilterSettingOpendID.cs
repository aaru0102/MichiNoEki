using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    public enum FilterSettingOpendID
    {
        /// <summary>
        /// 設定なし
        /// </summary>
        None = 0x00,

        /// <summary>
        /// 未オープン
        /// </summary>
        NotOpened = 0x01,

        /// <summary>
        /// オープン済み
        /// </summary>
        Opened = 0x02,
    }
}
