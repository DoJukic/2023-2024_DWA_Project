using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DBScaffold.Models;
using Microsoft.AspNetCore.Authorization;
using MVC_Module.Systems;

namespace MVC_Module.Controllers
{
    [Authorize(Roles = nameof(Administrator))]
    public class SecUserBorrowingReservationController : Controller
    {
        private readonly DwaContext _context;

        public SecUserBorrowingReservationController(DwaContext context)
        {
            _context = context;
        }

        // GET: UserBorrowingReservation
        public async Task<IActionResult> Index()
        {
            var dwaContext = _context.UserBorrowingReservations.Include(u => u.Bllink).Include(u => u.User);
            return View(await dwaContext.ToListAsync());
        }

        // GET: UserBorrowingReservation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBorrowingReservation = await _context.UserBorrowingReservations
                .Include(u => u.Bllink)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Idreservation == id);
            if (userBorrowingReservation == null)
            {
                return NotFound();
            }

            return View(userBorrowingReservation);
        }

        // GET: UserBorrowingReservation/Create
        public IActionResult Create()
        {
            List<BookLocationLink> BLLinks = _context.BookLocationLinks.ToList();
            ViewData["BllinkId"] = new SelectList(BLLinks.Where(x => BookReservationSystem.CheckCurrentBookAvailability(x.Idbllink) ?? false), "Idbllink", "Idbllink");
            ViewData["UserId"] = new SelectList(_context.Users, "Iduser", "Name");
            return View();
        }

        // POST: UserBorrowingReservation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idreservation,BllinkId,UserId,DateBorrowed,DateReturned")] UserBorrowingReservation userBorrowingReservation)
        {
            ModelState.Remove(nameof(userBorrowingReservation.Bllink));
            ModelState.Remove(nameof(userBorrowingReservation.User));
            ModelState.Remove(nameof(userBorrowingReservation.DateReserved));
            ModelState.Remove(nameof(userBorrowingReservation.DateExpiration));

            if (ModelState.IsValid)
            {
                if (BookReservationSystem.TryReserveBook(userBorrowingReservation.BllinkId, userBorrowingReservation.UserId) ?? false)
                    return RedirectToAction(nameof(Index));
            }

            ViewData["BllinkId"] = new SelectList(_context.BookLocationLinks, "Idbllink", "Idbllink", userBorrowingReservation.BllinkId);
            ViewData["UserId"] = new SelectList(_context.Users, "Iduser", "Name", userBorrowingReservation.UserId);
            return View(userBorrowingReservation);
        }

        // GET: UserBorrowingReservation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBorrowingReservation = await _context.UserBorrowingReservations.FindAsync(id);
            if (userBorrowingReservation == null)
            {
                return NotFound();
            }
            ViewData["BllinkId"] = new SelectList(_context.BookLocationLinks, "Idbllink", "Idbllink", userBorrowingReservation.BllinkId);
            ViewData["UserId"] = new SelectList(_context.Users, "Iduser", "Name", userBorrowingReservation.UserId);
            return View(userBorrowingReservation);
        }

        // POST: UserBorrowingReservation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idreservation,BllinkId,UserId,DateReserved,DateExpiration,DateBorrowed,DateReturned")] UserBorrowingReservation userBorrowingReservation)
        {
            if (id != userBorrowingReservation.Idreservation)
            {
                return NotFound();
            }

            ModelState.Remove(nameof(userBorrowingReservation.Bllink));
            ModelState.Remove(nameof(userBorrowingReservation.User));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userBorrowingReservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserBorrowingReservationExists(userBorrowingReservation.Idreservation))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BllinkId"] = new SelectList(_context.BookLocationLinks, "Idbllink", "Idbllink", userBorrowingReservation.BllinkId);
            ViewData["UserId"] = new SelectList(_context.Users, "Iduser", "Name", userBorrowingReservation.UserId);
            return View(userBorrowingReservation);
        }

        // GET: UserBorrowingReservation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBorrowingReservation = await _context.UserBorrowingReservations
                .Include(u => u.Bllink)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Idreservation == id);
            if (userBorrowingReservation == null)
            {
                return NotFound();
            }

            return View(userBorrowingReservation);
        }

        // POST: UserBorrowingReservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userBorrowingReservation = await _context.UserBorrowingReservations.FindAsync(id);
            if (userBorrowingReservation != null)
            {
                _context.UserBorrowingReservations.Remove(userBorrowingReservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserBorrowingReservationExists(int id)
        {
            return _context.UserBorrowingReservations.Any(e => e.Idreservation == id);
        }
    }
}
