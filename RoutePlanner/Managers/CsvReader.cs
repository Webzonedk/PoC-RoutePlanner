using RoutePlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;

namespace RoutePlanner.Managers
{
    internal class CsvReader
    {
        //public List<Address> LoadAddressesFromCsv(string filepath)
        public static List<ImportAddress> LoadAddressesFromCsv()
        {
            var filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "adresser_Ringsted.csv");


            var addresses = new List<ImportAddress>();
            var lines = File.ReadAllLines(filepath, Encoding.GetEncoding(28591));
            var headers = lines[0].Split(';');
            var dataLines = lines.Skip(1);

            // Get column indices based on headers
            var vejnavnIndex = Array.IndexOf(headers, "vejnavn");
            var husnrIndex = Array.IndexOf(headers, "husnr");
            var supplerendebynavnIndex = Array.IndexOf(headers, "supplerendebynavn");
            var postnrIndex = Array.IndexOf(headers, "postnr");
            var postnrnavnIndex = Array.IndexOf(headers, "postnrnavn");
            var wgs84koordinat_breddeIndex = Array.IndexOf(headers, "wgs84koordinat_bredde");
            var wgs84koordinat_laengdeIndex = Array.IndexOf(headers, "wgs84koordinat_laengde");

            // Check if any column index is -1 (not found)
            var allIndices = new[] { vejnavnIndex, husnrIndex, supplerendebynavnIndex, postnrIndex, postnrnavnIndex, wgs84koordinat_breddeIndex, wgs84koordinat_laengdeIndex };
            if (allIndices.Any(index => index == -1))
            {
                throw new Exception("One or more column headers were not found in the CSV file.");
            }

            foreach (var line in dataLines)
            {
                var columns = line.Split(';');

                // Check if the number of columns in the line matches the expected number
                if (columns.Length < headers.Length)
                {
                    throw new Exception($"Line '{line}' has fewer columns than expected.");
                }

                addresses.Add(new ImportAddress
                {
                    Vejnavn = columns[vejnavnIndex],
                    Husnr = columns[husnrIndex],
                    Supplerendebynavn = columns[supplerendebynavnIndex],
                    Postnr = columns[postnrIndex],
                    Postnrnavn = columns[postnrnavnIndex],
                    Wgs84koordinatBredde = columns[23],
                    Wgs84koordinatLængde = columns[24]
                });
            }

            return addresses;
        }

    }
}
