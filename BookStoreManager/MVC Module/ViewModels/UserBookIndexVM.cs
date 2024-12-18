using System.ComponentModel;

namespace MVC_Module.ViewModels
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public class UserBookIndexVM
    {
        public int IDReservation { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? Genre { get; set; }

        public string Location { get; set; }

        [DisplayName("Reservation Date")]
        public DateTimeOffset DateReserved { get; set; }

        [DisplayName("Expiration Date")]
        public DateTimeOffset DateExpiration { get; set; }

        [DisplayName("Date Borrowed")]
        public DateTimeOffset? DateBorrowed { get; set; }

        [DisplayName("Date Returned")]
        public DateTimeOffset? DateReturned { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}
