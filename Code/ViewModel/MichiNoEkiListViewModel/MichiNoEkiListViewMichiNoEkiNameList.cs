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
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">名称</param>
        public MichiNoEkiListViewMichiNoEkiNameList(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
