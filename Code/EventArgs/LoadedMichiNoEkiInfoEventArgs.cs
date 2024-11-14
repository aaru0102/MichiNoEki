namespace RoadsideStationApp
{
    public class LoadedMichiNoEkiInfoEventArgs : EventArgs
    {
        public List<MichiNoEkiInfo> MichiNoEkiInfoList { get; set; }

        public LoadedMichiNoEkiInfoEventArgs(List<MichiNoEkiInfo> michiNoEkiInfoList)
        {
            MichiNoEkiInfoList = michiNoEkiInfoList;
        }
    }
}
