using PrintSystem.Models;
using System.Text.Json;

namespace MVC_PrintSystem.Services
{
    public class WebAPIService : IWebAPIService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<WebAPIService> _logger;
        private readonly string _baseUrl;

        // SOLUTION : Pas d'injection d'HttpClient, on le crée nous-mêmes
        public WebAPIService(IConfiguration configuration, ILogger<WebAPIService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _baseUrl = "https://localhost:7048/"; // URL fixe pour éviter tout problème de config

            _logger.LogInformation($"WebAPIService initialized with BaseUrl: {_baseUrl}");
        }

        // Méthode pour créer un HttpClient propre à chaque appel
        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            client.BaseAddress = new Uri(_baseUrl);
            return client;
        }

        public async Task<ApiResponse> ProcessOnlinePaymentAsync(string username, float amount)
        {
            using var httpClient = CreateHttpClient();

            try
            {
                var request = new { Username = username, Amount = amount };

                _logger.LogInformation($"Calling API: {_baseUrl}api/payment/online");
                _logger.LogInformation($"Request: Username={username}, Amount={amount}");

                // Utilisation d'URL relative maintenant que BaseAddress est définie
                var response = await httpClient.PostAsJsonAsync("api/payment/online", request);

                _logger.LogInformation($"Response Status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"Response Content: {content}");

                    var result = JsonSerializer.Deserialize<ApiResponse>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return result ?? new ApiResponse { Success = false, ErrorMessage = "Empty response" };
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"API Error: {response.StatusCode} - {errorContent}");

                return new ApiResponse
                {
                    Success = false,
                    ErrorMessage = $"API Error: {response.StatusCode}"
                };
            }
            catch (HttpRequestException ex)
            {
                var errorMsg = $"Impossible de contacter l'API sur {_baseUrl}. Vérifiez que WebAPI_PrintSystem fonctionne. Erreur: {ex.Message}";
                _logger.LogError(errorMsg);
                return new ApiResponse { Success = false, ErrorMessage = errorMsg };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur inattendue: {ex.Message}");
                return new ApiResponse { Success = false, ErrorMessage = $"Erreur: {ex.Message}" };
            }
        }

        public async Task<ApiResponse> AddAmountAsync(string username, float quotas)
        {
            using var httpClient = CreateHttpClient();

            try
            {
                var request = new { Username = username, Quotas = quotas };
                var response = await httpClient.PostAsJsonAsync("api/quota/add", request);

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
            using var httpClient = CreateHttpClient();

            try
            {
                var response = await httpClient.GetAsync($"api/quota/available/{username}");

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

        public async Task<List<User>> GetFacultyStudentsAsync(string faculty)
        {
            using var httpClient = CreateHttpClient();

            try
            {
                var response = await httpClient.GetAsync($"api/users/faculty/{faculty}");

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
            using var httpClient = CreateHttpClient();

            try
            {
                var response = await httpClient.PostAsJsonAsync("api/faculty/add", new { Name = facultyName });

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<ApiResponse>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ??
                        new ApiResponse { Success = false, ErrorMessage = "Empty response" };
                }

                return new ApiResponse { Success = false, ErrorMessage = "API error" };
            }
            catch (Exception ex)
            {
                return new ApiResponse { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<string> GetUsernameAsync(string uid)
        {
            using var httpClient = CreateHttpClient();

            try
            {
                var response = await httpClient.GetAsync($"api/users/username/{uid}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<JsonElement>(content);

                    if (result.TryGetProperty("Username", out var usernameProperty))
                    {
                        return usernameProperty.GetString() ?? "unknown";
                    }
                }

                return "unknown";
            }
            catch
            {
                return "unknown";
            }
        }

        public async Task<User> GetUserDetailsAsync(string username)
        {
            using var httpClient = CreateHttpClient();

            try
            {
                var response = await httpClient.GetAsync($"api/users/details/{username}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<User>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ??
                        new User { Username = username, Role = "Student" };
                }

                return new User { Username = username, Role = "Student" };
            }
            catch
            {
                return new User { Username = username, Role = "Student" };
            }
        }

        // Méthodes supplémentaires requises par l'interface
        public async Task<List<FacultyStudent>> GetFacultyStudentsDetailedAsync(string faculty)
        {
            return new List<FacultyStudent>();
        }

        public async Task<ApiResponse> AllocateFacultyQuotaAsync(string username, float amount, string allocatedBy, string reason)
        {
            return new ApiResponse { Success = false, ErrorMessage = "Not implemented" };
        }

        public async Task<List<PaymentTransaction>> GetPaymentHistoryAsync(string username)
        {
            return new List<PaymentTransaction>();
        }

        public async Task<object> GetFacultyQuotaSummaryAsync(string faculty)
        {
            return new { };
        }
    }
}