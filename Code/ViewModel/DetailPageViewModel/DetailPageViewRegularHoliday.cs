using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    /// <summary>
    /// 定休日
    /// </summary>
    public class DetailPageViewRegularHoliday
    {
        private Dictionary<int, string> _dayOfTheWeekNameDic = new Dictionary<int, string>()
        {
            { 1, "月曜日" },
            { 2, "火曜日" },
            { 3, "水曜日" },
            { 4, "木曜日" },
            { 5, "金曜日" },
            { 6, "土曜日" },
            { 7, "日曜日" },
        };

        /// 曜日の名前
        /// </summary>
        public string DayOfTheWeekName { get; set; } = string.Empty;

        /// <summary>
        /// 休み
        /// </summary>
        public AutoNotifyProperty<bool> Close { get; set; } = new AutoNotifyProperty<bool>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DetailPageViewRegularHoliday(int dayNo)
        {
            DayOfTheWeekName = _dayOfTheWeekNameDic[dayNo];
        }
    }
}
