using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DBScaffold.Models;
using Microsoft.AspNetCore.Authorization;

namespace MVC_Module.Controllers
{
    [Authorize(Roles = nameof(Administrator))]
    public class SecAdministratorController : Controller
    {
        private readonly DwaContext _context;

        public SecAdministratorController(DwaContext context)
        {
            _context = context;
        }

        // GET: Administrators
        public async Task<IActionResult> Index()
        {
            var dwaContext = _context.Administrators.Include(a => a.User);
            return View(await dwaContext.ToListAsync());
        }

        // GET: Administrators/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Iduser", "Name");
            return View();
        }

        // POST: Administrators/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId")] Administrator administrator)
        {
            ModelState.Remove(nameof(Administrator.User));
            if (ModelState.IsValid && _context.Administrators.FirstOrDefault(x => x.UserId == administrator.UserId) is null)
            {
                _context.Add(administrator);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Administrator already exists!");
            ViewData["UserId"] = new SelectList(_context.Users, "Iduser", "Name", administrator.UserId);
            return View(administrator);
        }

        // GET: Administrators/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrator = await _context.Administrators
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (administrator == null)
            {
                return NotFound();
            }

            return View(administrator);
        }

        // POST: Administrators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var administrator = await _context.Administrators.FindAsync(id);
            if (administrator != null)
            {
                _context.Administrators.Remove(administrator);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdministratorExists(int id)
        {
            return _context.Administrators.Any(e => e.UserId == id);
        }
    }
}
