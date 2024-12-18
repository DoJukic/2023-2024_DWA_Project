using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Module.ViewModels;
using System.Security.Claims;
using DBScaffold.Models;
using FIS_API.Security;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using MVC_Module.AutoMapper;

namespace MVC_Module.Controllers
{
    public class UserController : Controller
    {
        private readonly DwaContext _context;

        public UserController(DwaContext context)
        {
            _context = context;
        }

        public IActionResult LogIn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult LogIn(string returnUrl, LoginVM loginVm)
        {
            // Try to get a user from database
            var existingLogin =
                _context
                    .Logins
                    .Include(x => x.User)
                    .ThenInclude(x => x.Administrator)
                    .FirstOrDefault(x => x.Email == loginVm.Email);

            if (existingLogin == null || existingLogin.User is null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View();
            }

            // Check is password hash matches
            var b64hash = PasswordHashProvider.GetHash(loginVm.Password, existingLogin.PasswordSalt);
            if (b64hash != existingLogin.PasswordHash)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View();
            }

            List<Claim> claims = new() {
                new Claim(ClaimTypes.Name, loginVm.Email),
                new Claim(ClaimTypes.Role, nameof(DBScaffold.Models.User))
            };

            if (existingLogin.User.Administrator != null)
                claims.Add(new Claim(ClaimTypes.Role, nameof(Administrator)));

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties();

            // We need to wrap async code here into synchronous since we don't use async methods
            Task.Run(async () =>
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties)
            ).GetAwaiter().GetResult();

            if (loginVm.ReturnUrl != null)
                return LocalRedirect(loginVm.ReturnUrl);

            if (existingLogin.User.Administrator is not null)
                return RedirectToAction("Index", "Home");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            Task.Run(async () =>
                await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme)
            ).GetAwaiter().GetResult();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Register(RegisterVM registerVM)
        {
            try
            {
                PropertyInfo[] properties = typeof(RegisterVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    object? propData = property.GetValue(registerVM, null);
                    if (propData != null && propData is string propStr)
                        property.SetValue(registerVM, propStr.Trim());
                }

                if (_context.Logins.FirstOrDefault(x => x.Email == registerVM.Email) is not null)
                {
                    ModelState.AddModelError("", $"E-mail \"{registerVM.Email}\" already exists.");
                    return View();
                }

                User user = StdMapper.Map<User>(registerVM);
                Login login = StdMapper.Map<Login>(registerVM);

                _context.Add(user);
                _context.SaveChanges();

                login.UserId = user.Iduser;

                _context.Add(login);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
