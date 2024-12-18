using DBScaffold.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;
using System.Collections.Generic;
using RESTful_Service_Module.Dtos;
using FIS_API.Security;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RESTful_Service_Module.Controllers
{
    [Route("api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DwaContext _context;

        public BookController(IConfiguration configuration, DwaContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet("get")]
        public ActionResult GetBooks(int? bookId)
        {
            try
            {
                if (bookId == null)
                    return Ok(BookInOutDto.GetBooksFromEnumerable(_context.Books));

                var tgt = _context.Books.Where(x => x.Idbook == bookId);

                if (tgt.Count() > 0)
                    return Ok(BookInOutDto.GetBookDtoFromBook(tgt.First()));

                return NotFound();
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("create")]
        [Authorize(Roles = "Administrator")]
        public ActionResult CreateBook(BookInOutDto bookIn)
        {
            try
            {
                if (bookIn.Name == null)
                    throw new BadHttpRequestException("Book name cannot be null!");

                var book = new Book();
                book.Name = bookIn.Name;
                book.Description = bookIn.Description;
                book.GenreId = bookIn.GenreId;

                if (bookIn.GenreId != null && !_context.Genres.Any(x => x.Idgenre == bookIn.GenreId))
                    throw new BadHttpRequestException("Book genre does not exist!");

                _context.Books.Add(book);
                _context.SaveChanges();

                return Ok();
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("update")]
        [Authorize(Roles = "Administrator")]
        public ActionResult UpdateBook(BookInOutDto bookIn)
        {
            try
            {
                if (bookIn.Idbook == null)
                    throw new BadHttpRequestException("You must provide a book ID!");
                if (bookIn.Name == null)
                    throw new BadHttpRequestException("Book name cannot be null!");

                var target = _context.Books.FirstOrDefault(x => x.Idbook == bookIn.Idbook);

                if (target == null)
                    throw new BadHttpRequestException("Book does not exist! Bad ID?");

                if (bookIn.GenreId != null && !_context.Genres.Any(x => x.Idgenre == bookIn.GenreId))
                    throw new BadHttpRequestException("Book genre does not exist!");

                target.Name = bookIn.Name;
                target.Description = bookIn.Description;
                target.GenreId = bookIn.GenreId;

                _context.SaveChanges();

                return Ok();
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("delete")]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteBook(BookInOutDto bookIn)
        {
            try
            {
                if (bookIn.Idbook == null)
                    throw new BadHttpRequestException("You must provide a book ID!");

                var target = _context.Books.FirstOrDefault(x => x.Idbook == bookIn.Idbook);

                if (target == null)
                    throw new BadHttpRequestException("Book does not exist! Bad ID?");

                _context.Books.Remove(target);
                _context.SaveChanges();

                return Ok();
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
