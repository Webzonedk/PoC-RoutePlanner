using RoutePlanner.DataSources;
using RoutePlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RoutePlanner.Managers
{
    internal class CalculateDistancesManager : DataService
    {
        private string _url => OpenRouteServiceUrl;

        /// <summary>
        /// Method to get the distances between all the addresses
        /// </summary>
        /// <param name="tempAddresses"></param>
        /// <returns>Returns a List of Distance </returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Distance>> GetDistancesAsync(List<Residence> tempAddresses)
        {
            using var httpClient = new HttpClient();
            var distancesList = new List<Distance>();
            const int maxRetries = 3; // Maximum number of retries for each request

            for (int i = 0; i < tempAddresses.Count; i++)
            {
                for (int j = i + 1; j < tempAddresses.Count; j++)
                {
                    var startAddress = tempAddresses[i];
                    var endAddress = tempAddresses[j];

                    var content = new StringContent($"{{\"coordinates\":[[{startAddress.Longitude},{startAddress.Latitude}],[{endAddress.Longitude},{endAddress.Latitude}]],\"instructions\":\"false\",\"preference\":\"recommended\"}}", Encoding.UTF8, "application/json");

                    HttpResponseMessage response = null;
                    int retryCount = 0;

                    while (retryCount < maxRetries)
                    {
                        try
                        {
                            response = await httpClient.PostAsync(_url, content);
                            if (response.IsSuccessStatusCode)
                            {
                                break; // Exit loop if request was successful
                            }
                        }
                        catch (HttpRequestException)
                        {
                            // exception logs, if needed
                        }

                        retryCount++;
                        await Task.Delay(1000 * retryCount); // Wait for a while before retrying (exponential increase) x 3 tries
                    }

                    if (response == null || !response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Failed to get route data after {maxRetries} attempts between Residence ID: {startAddress.ID} and Residence ID: {endAddress.ID}");
                    }

                    var responseBody = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var jsonResponse = JsonSerializer.Deserialize<DistanceApiResponseClasses.ApiResponse>(responseBody, options);

                    if (jsonResponse?.Routes == null || !jsonResponse.Routes.Any())
                    {
                        throw new Exception($"Routes data is missing in the API response between Residence ID: {startAddress.ID} and Residence ID: {endAddress.ID}");
                    }

                    var distance = new Distance
                    {
                        ResidenceOneID = startAddress.ID,
                        ResidenceTwoID = endAddress.ID,
                        Duration = (float)jsonResponse.Routes[0].Summary.Duration,
                        DistanceInMeters = (float)jsonResponse.Routes[0].Summary.Distance
                    };

                    distancesList.Add(distance);
                }
            }
            return distancesList;
        }
    }
}
