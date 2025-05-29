using System.Text.Json;
using PrintSystem.Models.Interfaces;

namespace WebAPI_PrintSystem.Services
{
    public class SAPHRService : ISAPHRService
    {
        private readonly HttpClient _httpClient;

        public SAPHRService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetUsernameAsync(string uid)
        {
            try
            {
                // For testing purposes, return a mock username
                await Task.Delay(100); // Simulate network call
                return $"user_{uid}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUsernameAsync: {ex.Message}");
                return $"user_{uid}"; // Fallback
            }
        }
    }
}