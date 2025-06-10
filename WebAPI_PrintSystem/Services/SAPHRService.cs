using PrintSystem.Models.Interfaces;

namespace WebAPI_PrintSystem.Services
{
    public class SAPHRService : PrintSystem.Models.Interfaces.ISAPHRService
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
                await Task.Delay(100);

                return uid switch
                {
                    "123" => "test.user",
                    "456" => "test.admin",
                    "789" => "joaquim.jonathan",
                    "101" => "marie.dupont",
                    "102" => "paul.martin",
                    _ => $"user_{uid}"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUsernameAsync: {ex.Message}");
                return $"user_{uid}";
            }
        }
    }
}