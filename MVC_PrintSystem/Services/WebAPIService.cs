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
                Console.WriteLine($"Request URI: {_httpClient.BaseAddress}api/quota/add");
                var request = new { Username = username, Quotas = quotas };
                var response = await _httpClient.PostAsJsonAsync("api/quota/add", request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<ApiResponse>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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
                    var result = JsonSerializer.Deserialize<ApiResponse>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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
                    var result = JsonSerializer.Deserialize<List<User>>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return result ?? new List<User>();
                }

                return new List<User>();
            }
            catch
            {
                return new List<User>();
            }
        }

        public async Task<ApiResponse> AddFacultyAsync(string facultyName)
        {
            var response = await _httpClient.PostAsJsonAsync("api/faculty/add", new { Name = facultyName });
            return await response.Content.ReadFromJsonAsync<ApiResponse>() ??
                   new ApiResponse { Success = false, ErrorMessage = "Empty response" };
        }

        public async Task<string> GetUsernameAsync(string uid)
        {
            var response = await _httpClient.GetAsync($"api/users/username/{uid}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<dynamic>(content);
                return result?.Username ?? "unknown";
            }
            return "unknown";
        }

        public async Task<User> GetUserDetailsAsync(string username)
        {
            var response = await _httpClient.GetAsync($"api/users/details/{username}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<User>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ??
                    new User { Username = username, Role = "Student" };
            }
            return new User { Username = username, Role = "Student" };
        }

        
        public async Task<List<FacultyStudent>> GetFacultyStudentsDetailedAsync(string faculty)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/faculties/students/{faculty}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<FacultyStudent>>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return result ?? new List<FacultyStudent>();
                }

                return new List<FacultyStudent>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving faculty students: {ex.Message}");
                return new List<FacultyStudent>();
            }
        }

        public async Task<ApiResponse> AllocateFacultyQuotaAsync(string username, float amount, string allocatedBy, string reason)
        {
            try
            {
                var request = new FacultyQuotaRequest
                {
                    Username = username,
                    Amount = amount,
                    AllocatedBy = allocatedBy,
                    Reason = reason
                };
                var response = await _httpClient.PostAsJsonAsync("api/faculties/allocate-quota", request); // Use relative URI

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<ApiResponse>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return result ?? new ApiResponse { Success = false, ErrorMessage = "Empty response" };
                }

                return new ApiResponse { Success = false, ErrorMessage = "API communication error" };
            }
            catch (Exception ex)
            {
                return new ApiResponse { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<List<PaymentTransaction>> GetPaymentHistoryAsync(string username)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/payment/history/{username}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<PaymentTransaction>>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return result ?? new List<PaymentTransaction>();
                }

                return new List<PaymentTransaction>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving payment history: {ex.Message}");
                return new List<PaymentTransaction>();
            }
        }

        public async Task<object> GetFacultyQuotaSummaryAsync(string faculty)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/faculties/quota-summary/{faculty}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<object>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new { };
                }

                return new { };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving faculty summary: {ex.Message}");
                return new { };
            }

        }
    }
}