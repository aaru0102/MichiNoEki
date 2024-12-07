using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    public class PinColorSettingVisitedData
    {
        /// <summary>
        /// ID
        /// </summary>
        public PinColorSettingVisitedID ID { get; set; }

        /// <summary>
        /// 色
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">テーブルデータ</param>
        public PinColorSettingVisitedData(PinColorSettingVisitedInfo info)
        {
            ID = (PinColorSettingVisitedID)info.ID;
            Color = Color.FromArgb(info.Color);
        }

        public PinColorSettingVisitedData(PinColorSettingVisitedID id, Color color)
        {
            ID = id;
            Color = color;
        }
    }
}
