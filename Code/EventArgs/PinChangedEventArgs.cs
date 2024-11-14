using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    public class PinChangedEventArgs : EventArgs
    {
        public MichiNoEkiPin Pin { get; set; }

        public PinChangedEventArgs(MichiNoEkiPin pin)
        {
            Pin = pin;
        }
    }
}
