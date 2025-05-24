using PrintSystem.Models;
using System.Text.Json;

namespace MVC_PrintSystem.Services
{
    public class WebAPIService : IWebAPIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public WebAPIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<ApiResponse> AddAmountAsync(string username, float quotas)
        {
            try
            {
                var request = new { Username = username, Quotas = quotas };
                var response = await _httpClient.PostAsJsonAsync("api/quota/add", request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<ApiResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return result ?? new ApiResponse { Success = false, ErrorMessage = "Empty response" };
                }

                return new ApiResponse { Success = false, ErrorMessage = "API communication error" };
            }
            catch (Exception ex)
            {
                return new ApiResponse { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<float> GetAvailableAmountAsync(string username)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/quota/available/{username}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (float.TryParse(content, out float result))
                    {
                        return result;
                    }
                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<ApiResponse> ProcessOnlinePaymentAsync(string username, float amount)
        {
            try
            {
                var request = new { Username = username, Amount = amount };
                var response = await _httpClient.PostAsJsonAsync("api/payment/online", request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<ApiResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return result ?? new ApiResponse { Success = false, ErrorMessage = "Empty response" };
                }

                return new ApiResponse { Success = false, ErrorMessage = "Payment error" };
            }
            catch (Exception ex)
            {
                return new ApiResponse { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<List<User>> GetFacultyStudentsAsync(string faculty)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/users/faculty/{faculty}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<User>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return result ?? new List<User>();
                }

                return new List<User>();
            }
            catch
            {
                return new List<User>();
            }
        }
    }
}