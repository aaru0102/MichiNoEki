using SQLite;

namespace RoadsideStationApp
{
    /// <summary>
    /// 道の駅情報テーブル
    /// </summary>
    public class MichiNoEkiInfoTable
    {
        /// <summary>
        /// ID
        /// </summary>
        [PrimaryKey]
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
        /// 開館時間リスト(シリアライズ用)
        /// </summary>
        public string OpenTimeListSerialized { get; set; } = string.Empty;

        /// <summary>
        /// 閉館時間リスト(シリアライズ用)
        /// </summary>
        public string CloseTimeListSerialized { get; set; } = string.Empty;

        /// <summary>
        /// 定休日リスト(シリアライズ用)
        /// </summary>
        public string CloseDayListSerialized { get; set; } = string.Empty;

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
    }
}
