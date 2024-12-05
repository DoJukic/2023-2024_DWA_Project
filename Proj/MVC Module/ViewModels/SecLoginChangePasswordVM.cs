using System.ComponentModel.DataAnnotations;

namespace MVC_Module.ViewModels
{
#pragma warning disable CS8618
    public class SecLoginChangePasswordVM
    {
        [Required(ErrorMessage = "Password is required")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password should be at least 8 characters long")]
        public string Password { get; set; }

        // https://stackoverflow.com/questions/21746910/compare-password-and-confirm-password-in-asp-net-mvc - Sender's answer, modified
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
#pragma warning restore CS8618
}
