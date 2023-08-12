using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using OpenRouteServiceApp;

namespace OpenRouteServiceApp
{
    class Program
    {
        private const string Url = "https://api.openrouteservice.org/v2/directions/driving-car";
        private const string AuthorizationKey = "5b3ce3597851110001cf62481e7382465ac54de19127d303893c63ba";

        static async Task Main()
        {
            var routeData = await GetRouteDataAsync();
            Debug.WriteLine($"Distance: {routeData.Distance}");
            Debug.WriteLine($"Duration: {routeData.Duration}");
            Debug.WriteLine($"Coordinates: {string.Join(", ", routeData.Coordinates)}");
            Debug.WriteLine($"Profile: {routeData.Profile}");
            Debug.WriteLine($"Preference: {routeData.Preference}");
        }
        /// <summary>
        /// Getting the routeData from the api and deserializing it into a RouteData object.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        static async Task<RouteData> GetRouteDataAsync()
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", AuthorizationKey);

            var content = new StringContent("{\"coordinates\":[[11.949467,55.322397],[12.118901,55.249683]],\"instructions\":\"false\",\"preference\":\"recommended\"}", Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(Url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var jsonResponse = JsonSerializer.Deserialize<ApiResponse>(responseBody, options);

            if (jsonResponse?.Routes == null || !jsonResponse.Routes.Any())
            {
                throw new Exception("Routes data is missing in the API response.");
            }

            var routeData = new RouteData(
                jsonResponse.Routes[0].Summary.Distance,
                jsonResponse.Routes[0].Summary.Duration,
                jsonResponse.Metadata.Query.Coordinates,
                jsonResponse.Metadata.Query.Profile,
                jsonResponse.Metadata.Query.Preference
            );

            return routeData;
        }
    }

    public record ApiResponse(List<Route> Routes, Metadata Metadata);

    public record Route(RouteSummary Summary);

    public record RouteSummary(double Distance, double Duration);

    public record Metadata(Query Query);

    public record Query(List<List<double>> Coordinates, string Profile, string Preference);
}
