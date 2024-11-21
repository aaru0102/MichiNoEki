namespace RoadsideStationApp
{
    public class AllUpdateVisibleEventArgs : EventArgs
    {
        public Dictionary<int, bool> VisibleDic { get; }

        public AllUpdateVisibleEventArgs(Dictionary<int, bool> visibleDic)
        {
            VisibleDic = visibleDic;
        }
    }
}
