using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DBScaffold.Models;
using Microsoft.AspNetCore.Authorization;
using MVC_Module.AutoMapper;
using MVC_Module.ViewModels;

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
        public IActionResult Index()
        {
            var locationLinks = _context.BookLocationLinks.Include(b => b.Book).Include(b => b.Location);
            var locationLinkVMs = StdMapper.Map<List<BookLocationLinkVM>>(locationLinks);
            return View(locationLinkVMs.ToList());
        }

        // GET: BookLocationLink/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookLocationLink = _context.BookLocationLinks
                .Include(b => b.Book)
                .Include(b => b.Location)
                .FirstOrDefault(m => m.Idbllink == id);

            if (bookLocationLink == null)
            {
                return NotFound();
            }

            return View(StdMapper.Map<BookLocationLinkVM>(bookLocationLink));
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
        public IActionResult Create([Bind("Idbllink,LocationId,BookId,Total")] BookLocationLinkVM bookLocationLinkVM)
        {
            ModelState.Remove(nameof(BookLocationLinkVM.Book));
            ModelState.Remove(nameof(BookLocationLinkVM.Location));
            if (ModelState.IsValid)
            {
                _context.Add(StdMapper.Map<BookLocationLink>(bookLocationLinkVM));
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Idbook", "Name", bookLocationLinkVM.BookId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Idlocation", "Name", bookLocationLinkVM.LocationId);
            return View(bookLocationLinkVM);
        }

        // GET: BookLocationLink/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookLocationLink = _context.BookLocationLinks.Find(id);
            if (bookLocationLink == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Idbook", "Name", bookLocationLink.BookId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Idlocation", "Name", bookLocationLink.LocationId);
            return View(StdMapper.Map<BookLocationLinkVM>(bookLocationLink));
        }

        // POST: BookLocationLink/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Idbllink,LocationId,BookId,Total")] BookLocationLinkVM bookLocationLinkVM)
        {
            if (id != bookLocationLinkVM.Idbllink)
            {
                return NotFound();
            }

            ModelState.Remove(nameof(BookLocationLinkVM.Book));
            ModelState.Remove(nameof(BookLocationLinkVM.Location));

            if (ModelState.IsValid)
            {
                var bookLocationLink = StdMapper.Map<BookLocationLink>(bookLocationLinkVM);
                try
                {
                    _context.Update(bookLocationLink);
                    _context.SaveChanges();
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

            ViewData["BookId"] = new SelectList(_context.Books, "Idbook", "Name", bookLocationLinkVM.BookId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Idlocation", "Name", bookLocationLinkVM.LocationId);
            return View(bookLocationLinkVM);
        }

        // GET: BookLocationLink/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookLocationLink = _context.BookLocationLinks
                .Include(b => b.Book)
                .Include(b => b.Location)
                .FirstOrDefault(m => m.Idbllink == id);

            if (bookLocationLink == null)
            {
                return NotFound();
            }

            return View(StdMapper.Map<BookLocationLinkVM>(bookLocationLink));
        }

        // POST: BookLocationLink/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var bookLocationLink = _context.BookLocationLinks.Find(id);
            if (bookLocationLink != null)
            {
                _context.BookLocationLinks.Remove(bookLocationLink);
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception x)
            {
                return StatusCode(500, x.InnerException?.Message ?? x.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookLocationLinkExists(int id)
        {
            return _context.BookLocationLinks.Any(e => e.Idbllink == id);
        }
    }
}
