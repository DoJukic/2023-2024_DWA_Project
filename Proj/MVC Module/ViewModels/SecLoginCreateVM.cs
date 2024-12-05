using System.ComponentModel.DataAnnotations;

namespace MVC_Module.ViewModels
{
    public class SecLoginCreateVM
    {
        //Login
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please provide a correct e-mail address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password should be at least 8 characters long")]
        public string Password { get; set; } = null!;

#pragma warning disable CS8618
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
#pragma warning restore CS8618

        [Required(ErrorMessage = "Target User is required")]
        public int UserId { get; set; }
    }
}
