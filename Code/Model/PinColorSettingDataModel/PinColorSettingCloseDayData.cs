using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    public class PinColorSettingCloseDayData
    {
        /// <summary>
        /// ID
        /// </summary>
        public PinColorSettingCloseDayID ID { get; set; }

        /// <summary>
        /// 色
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">テーブルデータ</param>
        public PinColorSettingCloseDayData(PinColorSettingCloseDayInfo info)
        {
            ID = (PinColorSettingCloseDayID)info.ID;
            Color = Color.FromArgb(info.Color);
        }
    }
}
