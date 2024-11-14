using System.ComponentModel;

namespace RoadsideStationApp
{
    public class AutoNotifyProperty<T> : INotifyPropertyChanged
    {
        private T _value;

        public event PropertyChangedEventHandler? PropertyChanged;

        public T Value
        {
            get => _value;
            set
            {
                if (!Equals(_value, value))
                {
                    _value = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                }
            }
        }

        public AutoNotifyProperty(T initialValue = default!)
        {
            _value = initialValue;
        }
    }
}
