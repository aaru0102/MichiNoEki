using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    public class PinColorSettingCloseTimeData
    {
        /// <summary>
        /// ID
        /// </summary>
        public PinColorSettingTimeID ID { get; set; }

        /// <summary>
        /// 有効状態
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 時間
        /// </summary>
        public TimeSpan Time { get; set; }

        /// <summary>
        /// 色
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">テーブルデータ</param>
        public PinColorSettingCloseTimeData(PinColorSettingCloseTimeInfo info)
        {
            ID = (PinColorSettingTimeID)info.ID;
            IsValid = info.IsValid;
            Time = TimeSpan.Parse(info.Time);
            Color = Color.FromArgb(info.Color);
        }
    }
}
