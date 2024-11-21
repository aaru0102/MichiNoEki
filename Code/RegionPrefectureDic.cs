using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    /// <summary>
    /// 地方-都道府県 名称辞書
    /// </summary>
    public static class RegionPrefectureDic
    {
        /// <summary>
        /// 地方-都道府県 名称辞書
        /// </summary>
        public static Dictionary<string, List<string>> Dic { get; } = new Dictionary<string, List<string>>()
        {
            {
                "北海道地方",
                new List<string>()
                {
                    "北海道"
                }
            },
            {
                "東北地方",
                new List<string>()
                {
                    "青森県",
                    "岩手県",
                    "宮城県",
                    "秋田県",
                    "山形県",
                    "福島県",
                }
            },
            {
                "関東地方",
                new List<string>()
                {
                    "茨城県",
                    "栃木県",
                    "群馬県",
                    "埼玉県",
                    "千葉県",
                    "東京都",
                    "神奈川県",
                    "山梨県",
                }
            },
            {
                "北陸地方",
                new List<string>()
                {
                    "新潟県",
                    "富山県",
                    "石川県",
                }
            },
            {
                "中部地方",
                new List<string>()
                {
                    "長野県",
                    "岐阜県",
                    "静岡県",
                    "愛知県",
                    "三重県",
                }
            },
            {
                "近畿地方",
                new List<string>()
                {
                    "福井県",
                    "滋賀県",
                    "京都府",
                    "大阪府",
                    "兵庫県",
                    "奈良県",
                    "和歌山県",
                }
            },
            {
                "中国地方",
                new List<string>()
                {
                    "鳥取県",
                    "島根県",
                    "岡山県",
                    "広島県",
                    "山口県",
                }
            },
            {
                "四国地方",
                new List<string>()
                {
                    "徳島県",
                    "香川県",
                    "愛媛県",
                    "高知県",
                }
            },
            {
                "九州地方",
                new List<string>()
                {
                    "福岡県",
                    "佐賀県",
                    "長崎県",
                    "熊本県",
                    "大分県",
                    "宮崎県",
                    "鹿児島県",
                    "沖縄県",
                }
            },
        };
    }
}
