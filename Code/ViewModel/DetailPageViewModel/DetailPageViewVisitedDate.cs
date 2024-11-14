using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    public class DetailPageViewVisitedDate
    {
        /// <summary>
        /// DateTime型
        /// </summary>
        public AutoNotifyProperty<DateTime?> DateTime { get; set; } = new AutoNotifyProperty<DateTime?>();

        /// <summary>
        /// String型
        /// </summary>
        public AutoNotifyProperty<string?> String { get; set; } = new AutoNotifyProperty<string?>(null);

        /// <summary>
        /// 日付セット
        /// </summary>
        /// <param name="dateTime">日付</param>
        public void SetDateTime(DateTime? dateTime)
        {
            DateTime.Value = dateTime;
            String.Value = dateTime == null? "0000/00/00" : ((DateTime)dateTime).ToString("yyyy/MM/dd");
        }
    }
}
