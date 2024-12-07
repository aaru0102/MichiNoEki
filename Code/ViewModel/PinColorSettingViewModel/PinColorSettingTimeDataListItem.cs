using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    public class PinColorSettingTimeDataListItem
    {
        /// <summary>
        /// ID
        /// </summary>
        public PinColorSettingTimeID ID { get; set; }

        /// <summary>
        /// 有効状態
        /// </summary>
        public AutoNotifyProperty<bool> IsValid { get; set; } = new AutoNotifyProperty<bool>(true);

        /// <summary>
        /// 時間
        /// </summary>
        public AutoNotifyProperty<TimeSpan> Time { get; set; } = new AutoNotifyProperty<TimeSpan>();

        /// <summary>
        /// 色
        /// </summary>
        public AutoNotifyProperty<Color> Color { get; set; } = new AutoNotifyProperty<Color>(Colors.White);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="time">時間</param>
        public PinColorSettingTimeDataListItem(PinColorSettingTimeID id, TimeSpan time)
        {
            ID = id;
            Time.Value = time;
        }
    }
}
