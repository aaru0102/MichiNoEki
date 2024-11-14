using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    public class ShowDetailMichiNoEkiInfoEvetArgs : EventArgs
    {
        public MichiNoEkiInfo MichiNoEkiInfo { get; set; }

        public ShowDetailMichiNoEkiInfoEvetArgs(MichiNoEkiInfo info)
        {
            MichiNoEkiInfo = info;
        }
    }
}
