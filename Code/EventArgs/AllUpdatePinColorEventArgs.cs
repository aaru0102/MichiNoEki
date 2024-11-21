namespace RoadsideStationApp
{
    public class AllUpdatePinColorEventArgs : EventArgs
    {
        public Dictionary<int, Color> PinColorDic { get; }

        public AllUpdatePinColorEventArgs(Dictionary<int, Color> pinColorDic)
        {
            PinColorDic = pinColorDic;
        }
    }
}
