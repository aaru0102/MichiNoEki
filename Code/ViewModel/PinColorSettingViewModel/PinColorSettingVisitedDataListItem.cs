using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    public class PinColorSettingVisitedDataListItem
    {
        /// <summary>
        /// ID
        /// </summary>
        public PinColorSettingVisitedID ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 色
        /// </summary>
        public AutoNotifyProperty<Color> Color { get; set; } = new AutoNotifyProperty<Color>(Colors.White);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">名称</param>
        public PinColorSettingVisitedDataListItem(PinColorSettingVisitedID id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
