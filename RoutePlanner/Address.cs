using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner
{
    public class Address
    {
        public string Vejnavn { get; set; }
        public string Husnr { get; set; }
        public string Supplerendebynavn { get; set; }
        public string Postnr { get; set; }
        public string Postnrnavn { get; set; }
        public string Wgs84koordinatBredde { get; set; }
        public string Wgs84koordinatLængde { get; set; }

        public override string ToString()
        {
            return $"{Vejnavn} {Husnr}, {Supplerendebynavn}, {Postnr} {Postnrnavn}, Bredde: {Wgs84koordinatBredde}, Længde: {Wgs84koordinatLængde}";
        }
    }

}
