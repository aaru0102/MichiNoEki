namespace RoadsideStationApp
{
    public class LoadedPinColorSettingInfoEventArgs : EventArgs
    {
        /// <summary>
        /// 有効なピンカラー設定
        /// </summary>
        private PinColorSettingID ValidPinColorSetting { get; set; }

        /// <summary>
        /// ピンカラー設定対象月
        /// </summary>
        private int PinColorMonth { get; set; }

        /// <summary>
        /// ピンカラー設定 訪問状態 設定内容
        /// </summary>
        private Dictionary<PinColorSettingVisitedID, PinColorSettingVisitedData> VisitedDataDic { get; set; } = new Dictionary<PinColorSettingVisitedID, PinColorSettingVisitedData>();

        /// <summary>
        /// ピンカラー設定 開館時間 設定内容
        /// </summary>
        private Dictionary<PinColorSettingTimeID, PinColorSettingOpenTimeData> OpenTimeDataDic { get; set; } = new Dictionary<PinColorSettingTimeID, PinColorSettingOpenTimeData>();

        /// <summary>
        /// ピンカラー設定 閉館時間 設定内容
        /// </summary>
        private Dictionary<PinColorSettingTimeID, PinColorSettingCloseTimeData> CloseTimeDataDic { get; set; } = new Dictionary<PinColorSettingTimeID, PinColorSettingCloseTimeData>();

        /// <summary>
        /// ピンカラー設定 定休日 設定内容
        /// </summary>
        private Dictionary<PinColorSettingCloseDayID, PinColorSettingCloseDayData> CloseDayDataDic { get; set; } = new Dictionary<PinColorSettingCloseDayID, PinColorSettingCloseDayData>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="validPinColorSetting">有効なピンカラー設定</param>
        /// <param name="pinColorMonth">ピンカラー設定対象月</param>
        /// <param name="visitedDataDic">ピンカラー設定 訪問状態 設定内容</param>
        /// <param name="openTimeDataDic">ピンカラー設定 訪問状態 開館時間</param>
        /// <param name="closeTimeDataDic">ピンカラー設定 訪問状態 閉館時間</param>
        /// <param name="closeDayDataDic">ピンカラー設定 定休日 設定内容</param>
        public LoadedPinColorSettingInfoEventArgs(PinColorSettingID validPinColorSetting, int pinColorMonth, Dictionary<PinColorSettingVisitedID, PinColorSettingVisitedData> visitedDataDic, Dictionary<PinColorSettingTimeID, PinColorSettingOpenTimeData> openTimeDataDic, Dictionary<PinColorSettingTimeID, PinColorSettingCloseTimeData> closeTimeDataDic, Dictionary<PinColorSettingCloseDayID, PinColorSettingCloseDayData> closeDayDataDic)
        {
            ValidPinColorSetting = validPinColorSetting;
            PinColorMonth = pinColorMonth;
            VisitedDataDic = visitedDataDic;
            OpenTimeDataDic = openTimeDataDic;
            CloseTimeDataDic = closeTimeDataDic;
            CloseDayDataDic = closeDayDataDic;
        }
    }
}
