using System.ComponentModel.DataAnnotations;

namespace MVC_Module.ViewModels
{
#pragma warning disable CS8618
    public class LoginVM
    {
        [EmailAddress]
        [Required(ErrorMessage = "E-mail is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password should be at least 8 characters long")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
#pragma warning restore CS8618
}