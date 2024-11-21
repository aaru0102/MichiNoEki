namespace RoadsideStationApp
{
    public class UpdateVisibleEventArgs : EventArgs
    {
        public int ID { get; }

        public bool Visible { get; }

        public UpdateVisibleEventArgs(int id, bool visible)
        {
            ID = id;
            Visible = visible;
        }
    }
}
