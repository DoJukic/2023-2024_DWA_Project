using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DBScaffold.Models;
using Microsoft.AspNetCore.Authorization;
using MVC_Module.ViewModels;
using FIS_API.Security;
using MVC_Module.AutoMapper;

namespace MVC_Module.Controllers
{
    [Authorize(Roles = nameof(Administrator))]
    public class SecLoginController : Controller
    {
        private readonly DwaContext _context;

        public SecLoginController(DwaContext context)
        {
            _context = context;
        }

        // GET: Logins
        public async Task<IActionResult> Index()
        {
            var dwaContext = _context.Logins.Include(l => l.User);
            return View(await dwaContext.ToListAsync());
        }

        // GET: Logins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var login = await _context.Logins
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.Idlogin == id);
            if (login == null)
            {
                return NotFound();
            }

            return View(login);
        }

        // GET: Logins/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Iduser", "Name");
            return View();
        }

        // POST: Logins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SecLoginCreateVM login)
        {
            if (_context.Logins.Any(x => x.Email == login.Email))
            {
                ModelState.AddModelError("", "Email already exists!");
                ViewData["UserId"] = new SelectList(_context.Users, "Iduser", "Name");
                return View();
            }

            _context.Logins.Add(StdMapper.Map<Login>(login));
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Logins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var login = await _context.Logins.FindAsync(id);
            if (login == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Iduser", "Name", login.UserId);
            return View(login);
        }

        // POST: Logins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idlogin,Email,UserId")] DBScaffold.Models.Login login)
        {
            if (id != login.Idlogin)
            {
                return NotFound();
            }

            ModelState.Remove(nameof(login.PasswordHash));
            ModelState.Remove(nameof(login.PasswordSalt));
            ModelState.Remove(nameof(login.User));

            if (ModelState.IsValid)
            {
                var currLogin = _context.Logins.FirstOrDefault(e => e.Idlogin == id);

                if (currLogin == null)
                    return NotFound("Error: no such user!");

                currLogin.Email = login.Email;
                currLogin.UserId = login.UserId;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Iduser", "Name", login.UserId);
            return View(login);
        }

        // GET: Logins/ChangePassword/5
        public async Task<IActionResult> ChangePassword(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var login = await _context.Logins.FindAsync(id);
            if (login == null)
            {
                return NotFound();
            }

            ViewData["UserId"] = new SelectList(_context.Users, "Iduser", "Name", login.UserId);
            return View();
        }

        // POST: Logins/ChangePassword/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(int id, SecLoginChangePasswordVM changePasswordVM)
        {
            var login = _context.Logins.FirstOrDefault(x => x.Idlogin == id);

            if (login == null)
                return NotFound();

            login.PasswordSalt = PasswordHashProvider.GetSalt();
            login.PasswordHash = PasswordHashProvider.GetHash(changePasswordVM.Password, login.PasswordSalt);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: Logins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var login = await _context.Logins
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.Idlogin == id);
            if (login == null)
            {
                return NotFound();
            }

            return View(login);
        }

        // POST: Logins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var login = await _context.Logins.FindAsync(id);
            if (login != null)
            {
                _context.Logins.Remove(login);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoginExists(int id)
        {
            return _context.Logins.Any(e => e.Idlogin == id);
        }
    }
}
