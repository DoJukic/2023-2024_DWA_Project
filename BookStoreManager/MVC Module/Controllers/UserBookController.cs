using System.Security.Claims;
using DBScaffold.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Module.AutoMapper;
using MVC_Module.Systems;
using MVC_Module.ViewModels;

namespace MVC_Module.Controllers
{
    [Authorize(Roles = nameof(DBScaffold.Models.User))]
    public class UserBookController : Controller
    {
        private readonly DwaContext _context;
        private readonly IConfiguration _configuration;

        public UserBookController(DwaContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            string? identifier = User?.Identity?.Name;

            if (identifier == null)
                return NotFound("No identifier!");

            // Damn
            var user = _context.Logins
                .Include(x => x.User)
                .ThenInclude(x => x.UserBorrowingReservations)
                .ThenInclude(x => x.Bllink)
                .ThenInclude(x => x.Location)
                .Include(x => x.User)
                .ThenInclude(x => x.UserBorrowingReservations)
                .ThenInclude(x => x.Bllink)
                .ThenInclude(x => x.Book)
                .ThenInclude(x => x.Genre)
                .FirstOrDefault(x => x.Email == identifier);

            if (user == null)
                return NotFound("No user!");

            List<UserBookIndexVM> reservations = new();

            foreach (var reservation in user.User.UserBorrowingReservations)
                reservations.Add(StdMapper.Map<UserBookIndexVM>(reservation));

            return View(reservations);
        }

        [HttpPost]
        public ActionResult Cancel(int targetReservation)
        {
            string? identifier = User?.Identity?.Name;

            if (identifier == null)
                return NotFound("No identifier!");

            var user = _context.Logins.Include(x => x.User).ThenInclude(x => x.UserBorrowingReservations).FirstOrDefault(x => x.Email == identifier);

            if (user == null)
                return NotFound("No user!");

            var reservation = user.User.UserBorrowingReservations.FirstOrDefault(x => x.Idreservation == targetReservation);

            if (reservation == null)
                return NotFound("Unknown reservation!");

            if (reservation.DateBorrowed != null)
                return BadRequest("This book has already been borrowed. Please return it if you want to 'cancel' your reservation.");

            _context.UserBorrowingReservations.Remove(reservation);
            _context.SaveChanges();

            return RedirectToAction("Index", "UserBook");
        }

        [HttpGet]
        public ActionResult Reserve(int? id)
        {
            var book = _context.Books
                .Include(x => x.Genre)
                .Include(x => x.BookLocationLinks)
                .ThenInclude(x => x.Location)
                .Include(x => x.BookLocationLinks)
                .ThenInclude(x => x.UserBorrowingReservations.Where(x => x.DateReturned == null))
                .FirstOrDefault(x => x.Idbook == id);

            if (book is null)
                return NotFound("Nothing");

            UserBookReserveVM reservationVM = StdMapper.Map<UserBookReserveVM>(book);

            List<KeyValuePair<int, string>> locations = new();
            foreach (var locationLink in book.BookLocationLinks)
                if (BookReservationSystem.CheckCurrentBookAvailability(locationLink.Idbllink) ?? false != false)
                    locations.Add(new(locationLink.Idbllink, locationLink.Location.Name));

            ViewData["LocationId"] = new SelectList(locations, "Key", "Value");

            return View(reservationVM);
        }

        [HttpPost]
        public ActionResult Reserve(int targetBLLLink)
        {
            string? identifier = User?.Identity?.Name;

            if (identifier == null)
                return NotFound("No identifier!");

            var user = _context.Logins.Include(x => x.User).FirstOrDefault(x => x.Email == identifier);

            if (user == null)
                return NotFound("No user!");

            var result = BookReservationSystem.TryReserveBook(targetBLLLink, user.UserId);

            if (result is null)
                return NotFound("No such book!");

            if (!result.Value)
                return BadRequest("Book not available.");

            return RedirectToAction("Search", "UserBook");
        }

        [HttpGet]
        public ActionResult Locations()
        {
            var locs = _context.Locations;

            return View(locs);
        }

        public ActionResult Search(UserBookSearchVM searchVm)
        {
            try
            {
                PrepareSearchViewmodel(searchVm);

                return View(searchVm);
            }
            catch (Exception ex)

            {
                throw ex;
            }
        }

        public ActionResult SearchPartial(UserBookSearchVM searchVm)
        {
            try
            {
                PrepareSearchViewmodel(searchVm);

                return PartialView("_SearchPartial", searchVm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PrepareSearchViewmodel(UserBookSearchVM searchVm)
        {
            IQueryable<Book> books = _context.Books
                .Include(x => x.Genre)
                .Include(x => x.BookLocationLinks)
                .ThenInclude(x => x.Location)
                .Include(x => x.BookLocationLinks)
                .ThenInclude(x => x.UserBorrowingReservations.Where(x => x.DateReturned == null));

            if (!string.IsNullOrEmpty(searchVm.Q))
            {
                books = books.Where(x => x.Name.Contains(searchVm.Q));
            }

            var filteredCount = books.Count();

            switch (searchVm.OrderBy.ToLower())
            {
                case "id":
                    books = books.OrderBy(x => x.Idbook);
                    break;
                case "name":
                    books = books.OrderBy(x => x.Name);
                    break;
                case "description":
                    books = books.OrderBy(x => x.Description);
                    break;
                case "genre":
                    books = books.OrderBy(x => x.GenreId);
                    break;
                case "availability":
                    books = books.OrderBy(x => !x.BookLocationLinks.Any(x => x.Total > x.UserBorrowingReservations.Count));
                    break;
            }

            books = books.Skip((searchVm.Page - 1) * searchVm.Size).Take(searchVm.Size); // if pages start from 1

            foreach(var testbook in books)
            {
                bool test = false;
                test = testbook.BookLocationLinks.Any(x => x.Total > x.UserBorrowingReservations.Count);
                test = true;
            }

            searchVm.Books = books.Select(x => StdMapper.Map<UserBookSearchDataVM>(x)).ToList();

            /*
            searchVm.Books =
                books.Select(x => new UserBookSearchDataVM
                {
                    Idbook = x.Idbook,
                    Name = x.Name,
                    Description = x.Description,
                    Genre = (x.Genre == null ? "None" : x.Genre.Name),
                    Availability = x.BookLocationLinks.Any(x => x.Total > x.UserBorrowingReservations.Count) ? "Available" : "Unavailable"
                })
                .ToList();
            */

            // BEGIN PAGER
            var expandPages = _configuration.GetValue<int>("Paging:ExpandPages");
            searchVm.LastPage = (int)Math.Ceiling(1.0 * filteredCount / searchVm.Size);
            searchVm.FromPager = searchVm.Page > expandPages ?
                searchVm.Page - expandPages :
                1;
            searchVm.ToPager = (searchVm.Page + expandPages) < searchVm.LastPage ?
                searchVm.Page + expandPages :
                searchVm.LastPage;
            // END PAGER
        }
    }
}
