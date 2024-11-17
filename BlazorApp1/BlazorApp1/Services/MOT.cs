//importing whats needed

using System.Net.Http.Headers; 
using System.Text.Json;
using System.Threading.Tasks; //asynchronous methods

namespace BupaMOTApp.Services
{
    public class MOTService
    {
        private readonly HttpClient _httpClient;

        public MOTService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<VehicleMOT?> GetMOTDetailsAsync(string registration)
        {
            
            // The base URL for the MOT API endpoint
            var url = $"trade/vehicles/mot-tests?registration={registration}";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("x-api-key", "fZi8YcjrZN1cGkQeZP7Uaa4rTxua8HovaswPuIno"); //this is the API key we need to be able to retrieve the information

            // GET request 
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // convert into object
                var content = await response.Content.ReadAsStringAsync();
                var vehicleMOT = JsonSerializer.Deserialize<VehicleMOT>(content);
                return vehicleMOT;
            }

            // If the response is not successful, return null
            return null;
        }
    }

    public class VehicleMOT
    {
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? Colour { get; set; }
        public string? MOTExpiryDate { get; set; }
        public int MileageAtLastMOT { get; set; }
    }
}


