using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    public class FilterSettingViewFilterDetailListItem
    {
        /// <summary>
        /// 親のフィルターID
        /// </summary>
        public FilterSettingID ParentID { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// フィルター名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// チェック状態
        /// </summary>
        public AutoNotifyProperty<bool> IsChecked { get; set; } = new AutoNotifyProperty<bool>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">フィルター名称</param>
        public FilterSettingViewFilterDetailListItem(FilterSettingID parentID, int id, string name)
        {
            ParentID = parentID;
            ID = id;
            Name = name;
            IsChecked.Value = true;
        }
    }
}
