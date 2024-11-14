namespace RoadsideStationApp
{
    public class UpdateMichiNoEkiInfoEventArgs : EventArgs
    {
        public MichiNoEkiInfo MichiNoEkiInfo { get; set; }
        public UpdateKind Kind { get; set; }

        public UpdateMichiNoEkiInfoEventArgs(MichiNoEkiInfo michiNoEkiInfo, UpdateKind kind)
        {
            MichiNoEkiInfo = michiNoEkiInfo;
            Kind = kind;
        }
    }
}
