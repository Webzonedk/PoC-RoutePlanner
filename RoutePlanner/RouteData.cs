using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRouteServiceApp
{
    public record RouteData(double Distance, double Duration, List<List<double>> Coordinates, string Profile, string Preference);
}
