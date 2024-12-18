using System.ComponentModel.DataAnnotations;

namespace MVC_Module.ViewModels
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public class UserProfileVM
    {
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name(s)")]
        public string? MiddleNames { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
#pragma warning restore CS8618
}
