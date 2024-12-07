namespace RoadsideStationApp
{
    public class LoadedPinColorSettingInfoEventArgs : EventArgs
    {
        /// <summary>
        /// 有効なピンカラー設定
        /// </summary>
        public PinColorSettingID ValidPinColorSetting { get; private set; }

        /// <summary>
        /// ピンカラー設定対象月
        /// </summary>
        public int PinColorMonth { get; private set; }

        /// <summary>
        /// 未オープン ピンカラー
        /// </summary>
        public Color NotOpendPinColor { get; private set; }

        /// <summary>
        /// スタンプ24時間押下OK ピンカラー
        /// </summary>
        public Color StampAllTimeOKPinColor { get; private set; }

        /// <summary>
        /// ピンカラー設定 訪問状態 設定内容
        /// </summary>
        public Dictionary<PinColorSettingVisitedID, PinColorSettingVisitedData> VisitedDataDic { get; private set; } = new Dictionary<PinColorSettingVisitedID, PinColorSettingVisitedData>();

        /// <summary>
        /// ピンカラー設定 開館時間 設定内容
        /// </summary>
        public Dictionary<PinColorSettingTimeID, PinColorSettingOpenTimeData> OpenTimeDataDic { get; private set; } = new Dictionary<PinColorSettingTimeID, PinColorSettingOpenTimeData>();

        /// <summary>
        /// ピンカラー設定 閉館時間 設定内容
        /// </summary>
        public Dictionary<PinColorSettingTimeID, PinColorSettingCloseTimeData> CloseTimeDataDic { get; private set; } = new Dictionary<PinColorSettingTimeID, PinColorSettingCloseTimeData>();

        /// <summary>
        /// ピンカラー設定 定休日 設定内容
        /// </summary>
        public Dictionary<PinColorSettingCloseDayID, PinColorSettingCloseDayData> CloseDayDataDic { get; private set; } = new Dictionary<PinColorSettingCloseDayID, PinColorSettingCloseDayData>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="validPinColorSetting">有効なピンカラー設定</param>
        /// <param name="pinColorMonth">ピンカラー設定対象月</param>
        /// <param name="visitedDataDic">ピンカラー設定 訪問状態 設定内容</param>
        /// <param name="openTimeDataDic">ピンカラー設定 訪問状態 開館時間</param>
        /// <param name="closeTimeDataDic">ピンカラー設定 訪問状態 閉館時間</param>
        /// <param name="closeDayDataDic">ピンカラー設定 定休日 設定内容</param>
        public LoadedPinColorSettingInfoEventArgs(PinColorSettingID validPinColorSetting, int pinColorMonth, Color notOpendPinColor, Color stampAllTimeOKPinColor, Dictionary<PinColorSettingVisitedID, PinColorSettingVisitedData> visitedDataDic, Dictionary<PinColorSettingTimeID, PinColorSettingOpenTimeData> openTimeDataDic, Dictionary<PinColorSettingTimeID, PinColorSettingCloseTimeData> closeTimeDataDic, Dictionary<PinColorSettingCloseDayID, PinColorSettingCloseDayData> closeDayDataDic)
        {
            ValidPinColorSetting = validPinColorSetting;
            PinColorMonth = pinColorMonth;
            NotOpendPinColor = notOpendPinColor;
            StampAllTimeOKPinColor = stampAllTimeOKPinColor;
            VisitedDataDic = visitedDataDic;
            OpenTimeDataDic = openTimeDataDic;
            CloseTimeDataDic = closeTimeDataDic;
            CloseDayDataDic = closeDayDataDic;
        }
    }
}
