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
    public class SecBookLocationLinkController : Controller
    {
        private readonly DwaContext _context;

        public SecBookLocationLinkController(DwaContext context)
        {
            _context = context;
        }

        // GET: BookLocationLink
        public async Task<IActionResult> Index()
        {
            var dwaContext = _context.BookLocationLinks.Include(b => b.Book).Include(b => b.Location);
            return View(await dwaContext.ToListAsync());
        }

        // GET: BookLocationLink/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookLocationLink = await _context.BookLocationLinks
                .Include(b => b.Book)
                .Include(b => b.Location)
                .FirstOrDefaultAsync(m => m.Idbllink == id);
            if (bookLocationLink == null)
            {
                return NotFound();
            }

            return View(bookLocationLink);
        }

        // GET: BookLocationLink/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Idbook", "Name");
            ViewData["LocationId"] = new SelectList(_context.Locations, "Idlocation", "Name");
            return View();
        }

        // POST: BookLocationLink/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idbllink,LocationId,BookId,Total")] BookLocationLink bookLocationLink)
        {
            ModelState.Remove(nameof(BookLocationLink.Book));
            ModelState.Remove(nameof(BookLocationLink.Location));
            if (ModelState.IsValid)
            {
                _context.Add(bookLocationLink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Idbook", "Name", bookLocationLink.BookId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Idlocation", "Name", bookLocationLink.LocationId);
            return View(bookLocationLink);
        }

        // GET: BookLocationLink/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookLocationLink = await _context.BookLocationLinks.FindAsync(id);
            if (bookLocationLink == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Idbook", "Name", bookLocationLink.BookId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Idlocation", "Name", bookLocationLink.LocationId);
            return View(bookLocationLink);
        }

        // POST: BookLocationLink/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idbllink,LocationId,BookId,Total")] BookLocationLink bookLocationLink)
        {
            if (id != bookLocationLink.Idbllink)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookLocationLink);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookLocationLinkExists(bookLocationLink.Idbllink))
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
            ViewData["BookId"] = new SelectList(_context.Books, "Idbook", "Name", bookLocationLink.BookId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Idlocation", "Name", bookLocationLink.LocationId);
            return View(bookLocationLink);
        }

        // GET: BookLocationLink/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookLocationLink = await _context.BookLocationLinks
                .Include(b => b.Book)
                .Include(b => b.Location)
                .FirstOrDefaultAsync(m => m.Idbllink == id);
            if (bookLocationLink == null)
            {
                return NotFound();
            }

            return View(bookLocationLink);
        }

        // POST: BookLocationLink/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookLocationLink = await _context.BookLocationLinks.FindAsync(id);
            if (bookLocationLink != null)
            {
                _context.BookLocationLinks.Remove(bookLocationLink);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookLocationLinkExists(int id)
        {
            return _context.BookLocationLinks.Any(e => e.Idbllink == id);
        }
    }
}
