using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    /// <summary>
    /// 都道府県 訪問率リストアイテム
    /// </summary>
    public class VisitedRateViewPrefectureListItem
    {
        /// <summary>
        /// 訪問状態Dic
        /// </summary>
        private Dictionary<int, bool> _visitedDic = new Dictionary<int, bool>();

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 訪問率
        /// </summary>
        public AutoNotifyProperty<uint> Rate { get; set; } = new AutoNotifyProperty<uint>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名称</param>
        public VisitedRateViewPrefectureListItem(string name)
        {
            Name = name;
            Rate.Value = 0;
        }

        /// <summary>
        /// 訪問率更新
        /// </summary>
        /// <param name="info">道の駅データ</param>
        public void UpdateRate(MichiNoEkiInfo info)
        {
            if (!_visitedDic.TryAdd(info.ID, info.IsVisited))
            {
                _visitedDic[info.ID] = info.IsVisited;
            }

            Rate.Value = (uint)Math.Round((double)_visitedDic.Values.Count(isVisited => isVisited == true) / _visitedDic.Count * 100, MidpointRounding.AwayFromZero);
        }
    }
}
