namespace RoadsideStationApp
{
    public class LoadedFilterSettingInfoEventArgs : EventArgs
    {
        /// <summary>
        /// フィルターデータ
        /// </summary>
        public Dictionary<FilterSettingID, FilterSettingData> FilterSettingsDic = new Dictionary<FilterSettingID, FilterSettingData>();

        public LoadedFilterSettingInfoEventArgs(Dictionary<FilterSettingID, FilterSettingData> filterSettingsDic)
        {
            FilterSettingsDic = filterSettingsDic;
        }
    }
}
