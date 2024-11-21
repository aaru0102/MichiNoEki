namespace RoadsideStationApp
{
    public class UpdatePinColorEventArgs : EventArgs
    {
        public int ID { get; }

        public Color Color { get; }

        public UpdatePinColorEventArgs(int id, Color color)
        {
            ID = id;
            Color = color;
        }
    }
}
