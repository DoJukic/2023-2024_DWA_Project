using DBScaffold.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Module.AutoMapper;
using MVC_Module.ViewModels;

namespace MVC_Module.Controllers
{
    [Authorize(Roles = nameof(DBScaffold.Models.User))]
    public class UserProfile : Controller
    {
        private readonly DwaContext _context;

        public UserProfile(DwaContext context)
        {
            _context = context;
        }

        public IActionResult ProfileDetails()
        {
            string? identifier = User?.Identity?.Name;

            if (identifier == null)
                return NotFound("No identifier!");

            var login = _context.Logins
                .Include(x => x.User)
                .FirstOrDefault(x => x.Email == identifier);

            if (login == null)
                return NotFound("No user!");

            UserProfileVM profileVM = new();

            StdMapper.Map(login, profileVM);
            StdMapper.Map(login.User, profileVM);

            return View(profileVM);
        }

        public JsonResult GetProfileData()
        {
            string? identifier = User?.Identity?.Name;

            if (identifier == null)
                return Json(null);

            var login = _context.Logins
                .Include(x => x.User)
                .FirstOrDefault(x => x.Email == identifier);

            if (login == null)
                return Json(null);

            UserProfileVM profileVM = new();

            StdMapper.Map(login, profileVM);
            StdMapper.Map(login.User, profileVM);

            return Json(new
            {
                profileVM.Email,
                profileVM.FirstName,
                profileVM.MiddleNames,
                profileVM.LastName,
            });
        }

        public IActionResult ProfileEdit()
        {
            string? identifier = User?.Identity?.Name;

            if (identifier == null)
                return NotFound("No identifier!");

            var login = _context.Logins
                .Include(x => x.User)
                .FirstOrDefault(x => x.Email == identifier);

            if (login == null)
                return NotFound("No user!");

            UserProfileVM profileVM = new();

            StdMapper.Map(login, profileVM);
            StdMapper.Map(login.User, profileVM);

            return View(profileVM);
        }

        [HttpPost]
        public IActionResult ProfileEdit(UserProfileVM profileVM)
        {
            string? identifier = User?.Identity?.Name;

            if (identifier == null)
                return NotFound("No identifier!");

            var login = _context.Logins
                .Include(x => x.User)
                .FirstOrDefault(x => x.Email == identifier);

            if (login == null)
                return NotFound("No user!");

            StdMapper.Map(profileVM, login);
            StdMapper.Map(profileVM, login.User);

            _context.SaveChanges();

            return View(profileVM);
        }
    }
}
