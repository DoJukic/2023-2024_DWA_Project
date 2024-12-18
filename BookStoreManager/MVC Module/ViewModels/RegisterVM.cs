using DBScaffold.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_Module.ViewModels
{
    public class RegisterVM
    {
        //Login
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please provide a correct e-mail address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password should be at least 8 characters long")]
        public string Password { get; set; } = null!;

#pragma warning disable CS8618 // Bruh shut up
        // https://stackoverflow.com/questions/21746910/compare-password-and-confirm-password-in-asp-net-mvc - Sender's answer, modified
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
#pragma warning restore CS8618

        //User
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name should be between 2 and 50 characters long")]
        public string Name { get; set; } = null!;

        public string? MiddleNames { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name should be between 2 and 50 characters long")]
        public string Surname { get; set; } = null!;
    }
}
