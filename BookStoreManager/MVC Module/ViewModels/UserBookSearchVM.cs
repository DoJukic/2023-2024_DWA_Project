

namespace MVC_Module.ViewModels
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public class UserBookSearchVM
    {
        public string Q { get; set; }
        public string OrderBy { get; set; } = "";
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public int LastPage { get; set; }
        public int FromPager { get; set; }
        public int ToPager { get; set; }
        public string Submit { get; set; }
        public IEnumerable<UserBookSearchDataVM> Books { get; set; }
    }
#pragma warning restore CS8618
}
