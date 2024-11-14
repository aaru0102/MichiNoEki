using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    /// <summary>
    /// アップデート種別
    /// </summary>
    public enum UpdateKind
    {
        /// <summary>
        /// 更新
        /// </summary>
        Update,

        /// <summary>
        /// 追加
        /// </summary>
        Add,

        /// <summary>
        /// 削除
        /// </summary>
        Delete,
    }
}
