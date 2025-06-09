using System.ComponentModel.DataAnnotations;

namespace MVC_PrintSystem.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Card ID is required")]
        public int CardId { get; set; }
    }
}