using DBScaffold.Models;

namespace MVC_Module.ViewModels
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public class UserBookSearchDataVM
    {
        public int Idbook { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? Genre { get; set; }

        public string Availability { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}
