using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DBScaffold.Models;

namespace MVC_Module.ViewModels
{
    public class BookLocationLinkVM
    {
        public int Idbllink { get; set; }

        [Display(Name = "Location")]
        public int LocationId { get; set; }

        [Display(Name = "Book")]
        public int BookId { get; set; }

        public int Total { get; set; }

        public virtual Location? Location { get; set; } = null!;

        public virtual Book? Book { get; set; } = null!;
    }
}
