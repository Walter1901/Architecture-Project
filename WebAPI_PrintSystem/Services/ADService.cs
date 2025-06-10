using PrintSystem.Models.Interfaces;

namespace WebAPI_PrintSystem.Services
{
    public class ADService : PrintSystem.Models.Interfaces.IADService
    {
        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            try
            {
                await Task.Delay(100);

                if (username == "joaquim.jonathan" && password == "welcome18")
                {
                    return true;
                }

                if (password == "test")
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AuthenticateAsync: {ex.Message}");
                return false;
            }
        }
    }
}