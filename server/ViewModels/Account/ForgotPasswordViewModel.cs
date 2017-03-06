using System.ComponentModel.DataAnnotations;

namespace MicroSB.Server.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
