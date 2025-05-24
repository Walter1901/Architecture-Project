namespace WebAPI_PrintSystem.Services
{
    public class ADService : IADService
    {
        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            try
            {
                // For testing purposes, allow specific test credentials
                await Task.Delay(100); // Simulate authentication process

                if (username == "joaquim.jonathan" && password == "welcome18")
                {
                    return true;
                }

                // For testing, allow any username with password "test"
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