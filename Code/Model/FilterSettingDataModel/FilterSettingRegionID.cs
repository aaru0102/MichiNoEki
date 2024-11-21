using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    public enum FilterSettingRegionID
    {
        /// <summary>
        /// 設定なし
        /// </summary>
        None = 0x000,

        /// <summary>
        /// 北海道地方
        /// </summary>
        Hokkaido = 0x001,

        /// <summary>
        /// 東北地方
        /// </summary>
        Tohoku = 0x002,

        /// <summary>
        /// 関東地方
        /// </summary>
        Kanto = 0x004,

        /// <summary>
        /// 北陸地方
        /// </summary>
        Hokuriku = 0x008,

        /// <summary>
        /// 中部地方
        /// </summary>
        Tyubu = 0x010,

        /// <summary>
        /// 近畿地方
        /// </summary>
        Kinki = 0x020,

        /// <summary>
        /// 中国地方
        /// </summary>
        Tyugoku = 0x040,

        /// <summary>
        /// 四国地方
        /// </summary>
        Shikoku = 0x080,

        /// <summary>
        /// 九州地方
        /// </summary>
        Kyusyu = 0x100,
    }
}
