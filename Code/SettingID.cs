using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    /// <summary>
    /// 設定データID
    /// </summary>
    public enum SettingID
    {
        /// <summary>
        /// フィルター設定
        /// </summary>
        Filter = 1,

        /// <summary>
        /// ピンカラー設定
        /// </summary>
        PinColor = 2,

        /// <summary>
        /// ピンカラー対象月
        /// </summary>
        PinColorMonth = 3,

        /// <summary>
        /// 未オープン ピンカラー
        /// </summary>
        NotOpendPinColor = 4,

        /// <summary>
        /// スタンプ24時間押下OK ピンカラー
        /// </summary>
        StampAllTimeOKPinColor = 5,
    }
}
