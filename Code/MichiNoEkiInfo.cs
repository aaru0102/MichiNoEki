using SQLite;

namespace RoadsideStationApp
{
    /// <summary>
    /// 道の駅情報
    /// </summary>
    public class MichiNoEkiInfo
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
        /// 緯度
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 経度
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 住所
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 地方
        /// </summary>
        public string Region { get; set; } = string.Empty;

        /// <summary>
        /// 都道府県
        /// </summary>
        public string Prefecture { get; set; } = string.Empty;

        /// <summary>
        /// 開館時間リスト(月～日)
        /// </summary>
        public List<TimeSpan?> OpenTimeList{ get; set; } = new List<TimeSpan?>();

        /// <summary>
        /// 閉館時間リスト(月～日)
        /// </summary>
        public List<TimeSpan?> CloseTimeList { get; set; } = new List<TimeSpan?>();

        /// <summary>
        /// 定休日リスト(月～日)
        /// </summary>
        public List<bool?> CloseDayList { get; set; } = new List<bool?>();

        /// <summary>
        /// スタンプ24時間押下OK
        /// </summary>
        public bool StampAllTimeOK { get; set; }

        /// <summary>
        /// オープン済み
        /// </summary>
        public bool IsOpened { get; set; }

        /// <summary>
        /// 訪問済み
        /// </summary>
        public bool IsVisited { get; set; }

        /// <summary>
        /// 訪問日
        /// </summary>
        public DateTime? VisitedDate { get; set; }

        /// <summary>
        /// 注意事項あり
        /// </summary>
        public bool Notice { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// ディープコピー
        /// </summary>
        /// <returns>コピーデータ</returns>
        public MichiNoEkiInfo DeepCopy()
        {
            MichiNoEkiInfo ret = new MichiNoEkiInfo()
            {
                ID = ID,
                Name = Name,
                Latitude = Latitude,
                Longitude = Longitude,
                Address = Address,
                Region = Region,
                Prefecture = Prefecture,
                CloseDayList = new List<bool?>(CloseDayList),
                StampAllTimeOK = StampAllTimeOK,
                IsOpened = IsOpened,
                IsVisited = IsVisited,
                VisitedDate = VisitedDate == null ? null : new DateTime(((DateTime)VisitedDate).Ticks),
                Notice = Notice,
                Comment = Comment,
            };

            foreach (var item in OpenTimeList)
            {
                ret.OpenTimeList.Add(item);
            }
            foreach (var item in CloseTimeList)
            {
                ret.CloseTimeList.Add(item);
            }

            return ret;
        }
    }
}
