using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    /// <summary>
    /// 道の駅名称リスト
    /// </summary>
    public class MichiNoEkiListViewMichiNoEkiNameList
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 訪問状態
        /// </summary>
        public AutoNotifyProperty<bool> IsVisited { get; set; } = new AutoNotifyProperty<bool>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">道の駅データ</param>
        public MichiNoEkiListViewMichiNoEkiNameList(MichiNoEkiInfo info)
        {
            ID = info.ID;
            Name = info.Name;
            IsVisited.Value = info.IsVisited;
        }
    }
}
