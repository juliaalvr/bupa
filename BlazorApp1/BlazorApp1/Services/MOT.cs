//importing whats needed

using System.Text.Json;
using System.Text.Json.Serialization; 

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
             // Relative URL for the MOT API endpoint
            var relativeUrl = $"trade/vehicles/mot-tests?registration={registration}";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("x-api-key", "fZi8YcjrZN1cGkQeZP7Uaa4rTxua8HovaswPuIno");
            
            
            //Console.WriteLine($"**DEBUG** API Key: {(_httpClient.DefaultRequestHeaders.Contains("x-api-key") ? "API Key Present" : "API Key Not Present")}");

            // GET request 
            var response = await _httpClient.GetAsync(relativeUrl);

            //Console.WriteLine($"**DEBUG** Response Status Code: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                // convert into object
                var content = await response.Content.ReadAsStringAsync();
                //Console.WriteLine($"**DEBUG** Response Content: {content}");

                //in case one registration has more than one vehicle
                var vehicles = JsonSerializer.Deserialize<List<VehicleMOT>>(content);
                var vehicleMOT = vehicles?.FirstOrDefault(); 

                return vehicleMOT;
            }

            // If the response is not successful, return null
            return null;
        }
    }

 public class VehicleMOT
    {
        [JsonPropertyName("registration")]
        public string? Registration { get; set; }

        [JsonPropertyName("make")]
        public string? Make { get; set; }

        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("firstUsedDate")]
        public string? FirstUsedDate { get; set; }

        [JsonPropertyName("fuelType")]
        public string? FuelType { get; set; }

        [JsonPropertyName("primaryColour")]
        public string? PrimaryColour { get; set; }

        [JsonPropertyName("motTests")]
        public List<MOTTest>? MotTests { get; set; }

        public string? MostRecentExpiryDate => 
            MotTests?.OrderByDescending(test => test.ExpiryDate).FirstOrDefault()?.ExpiryDate;

        public string? MostRecentMileage => 
            MotTests?.OrderByDescending(test => 
                int.TryParse(test.OdometerValue, out var mileage) ? mileage : 0)
                     .FirstOrDefault()?.OdometerValue;
    }

    public class MOTTest
    {
        [JsonPropertyName("completedDate")]
        public string? CompletedDate { get; set; }

        [JsonPropertyName("testResult")]
        public string? TestResult { get; set; }

        [JsonPropertyName("expiryDate")]
        public string? ExpiryDate { get; set; }

        [JsonPropertyName("odometerValue")]
        public string? OdometerValue { get; set; }

        [JsonPropertyName("odometerUnit")]
        public string? OdometerUnit { get; set; }

        [JsonPropertyName("motTestNumber")]
        public string? MotTestNumber { get; set; }

        [JsonPropertyName("rfrAndComments")]
        public List<RfrAndComment>? RfrAndComments { get; set; }
    }

    public class RfrAndComment
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
}



