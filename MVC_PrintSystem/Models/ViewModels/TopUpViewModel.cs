using System.ComponentModel.DataAnnotations;

namespace MVC_PrintSystem.Models.ViewModels
{
    public class TopUpViewModel
    {
        [Required(ErrorMessage = "Amount is required")]
        [Range(0.1, 100.0, ErrorMessage = "Amount must be between 0.1 and 100 CHF")]
        public float Amount { get; set; }
    }
}
