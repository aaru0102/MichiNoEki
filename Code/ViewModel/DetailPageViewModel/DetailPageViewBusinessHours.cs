using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    /// <summary>
    /// 営業時間
    /// </summary>
    public class DetailPageViewBusinessHours
    {
        /// <summary>
        /// 月の名前
        /// </summary>
        public string MonthName { get; set; } = string.Empty;

        /// <summary>
        /// 開館時間
        /// </summary>
        public AutoNotifyProperty<TimeSpan> OpenTime { get; set; } = new AutoNotifyProperty<TimeSpan>(TimeSpan.Zero);

        /// <summary>
        /// 閉館時間
        /// </summary>
        public AutoNotifyProperty<TimeSpan> CloseTime { get; set; } = new AutoNotifyProperty<TimeSpan>(TimeSpan.Zero);

        /// <summary>
        /// 開館時間(文字列)
        /// </summary>
        public AutoNotifyProperty<string> OpenTimeString { get; set; } = new AutoNotifyProperty<string>("0:00");

        /// <summary>
        /// 閉館時間(文字列)
        /// </summary>
        public AutoNotifyProperty<string> CloseTimeString { get; set; } = new AutoNotifyProperty<string>("0:00");

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mounthNo">〇月</param>
        public DetailPageViewBusinessHours(int mounthNo)
        {
            MonthName = mounthNo.ToString() + "月";
        }

        /// <summary>
        /// 開館時間セット
        /// </summary>
        /// <param name="time">時間</param>
        public void SetOpenTime(TimeSpan? time)
        {
            OpenTime.Value = time ?? TimeSpan.Zero;
            OpenTimeString.Value = time == null ? "0:00" : ((TimeSpan)time).ToString(@"hh\:mm");
        }

        /// <summary>
        /// 閉館時間セット
        /// </summary>
        /// <param name="time">時間</param>
        public void SetCloseTime(TimeSpan? time)
        {
            CloseTime.Value = time ?? TimeSpan.Zero;
            CloseTimeString.Value = time == null ? "0:00" : ((TimeSpan)time).ToString(@"hh\:mm");
        }
    }
}
