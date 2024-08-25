using DataNexus.Data;
using DataNexus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DataNexus.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookRepository _bookRepository;

        public BooksController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            _bookRepository = new BookRepository(connectionString);
        }

        public IActionResult Index()
        {
            var books = _bookRepository.GetBooks();
            return View(books);
        }

        public IActionResult AddOrEdit(int id = 0)
        {
            BookViewModel book = id == 0 ? new BookViewModel() : _bookRepository.GetBookByID(id);
            return View(book);
        }

        [HttpPost]
        public IActionResult AddOrEdit(BookViewModel book)
        {
            if (ModelState.IsValid)
            {
                _bookRepository.AddOrEditBook(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        public IActionResult Delete(int id)
        {
            var book = _bookRepository.GetBookByID(id);
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _bookRepository.DeleteBook(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
