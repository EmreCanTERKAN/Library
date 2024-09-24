using Library.Data;
using Library.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContex _contex;
        public BooksController(LibraryContex contex)
        {
            _contex = contex;
        }

        [HttpPost]
        public ActionResult<Book> Post([FromBody] Book book)
        {
            _contex.Books.Add(book);
            _contex.SaveChanges();
            return Ok(book);
        }

        [HttpPut("id")]
        public ActionResult Put(int id, [FromBody] Book book)
        {
            var current = _contex.Books.FirstOrDefault(b => b.Id == id);
            if (current is null)
            {
                return NotFound();
            }
            current.Title = book.Title;
            current.LastBorrowedDate = book.LastBorrowedDate;
            current.Price = book.Price;
            current.CategoryId = book.CategoryId;

            _contex.SaveChanges();
            return NoContent();


        }

        [HttpGet]
        public ActionResult<List<Book>> GetAll ()
        {
            var reccentBook = _contex.Books
                              .Where(b => b.PublicationYear > 2020)
                              .OrderByDescending(b => b.PublicationYear)
                              .Take(10)
                              .ToList();


            var book = _contex.Books.FromSqlRaw("Select * From where publicationYear > {0}", 2020).ToList();



            return Ok(book);
        }
        [HttpDelete("id")]
        public ActionResult Delete (int id)
        {
            var current = _contex.Books.FirstOrDefault(x => x.Id == id);
            if (current is null)
            {
                return NotFound();
            }
            _contex.Books.Remove(current);
            _contex.SaveChanges();
            return NoContent();
        }

    }
}
