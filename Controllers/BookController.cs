using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PB503_Libary_Managment_System_ASP.NET.Data;
using PB503_Libary_Managment_System_ASP.NET.Models;
using PB503_Libary_Managment_System_ASP.NET.View_Models.BookCategoryVM;
using PB503_Libary_Managment_System_ASP.NET.View_Models.BookVM;

namespace PB503_Libary_Managment_System_ASP.NET.Controllers
{
    public class BookController : Controller
    {
        private readonly LibaryDbContext _db;
        public BookController(LibaryDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var getDatas = await _db.Books.Where(item => !item.isDeleted).ToListAsync();
            var mapToVM = getDatas.Select(item => new BookVM()
            {
                ID = item.ID,
                CreatedDate = item.CreatedDate,
                UpdatedDate = item.UpdatedDate,
                Title = item.Title,
                Authors = item.Authors,
                Publisher = item.Publisher,
                CategoryId = item.ID,
                Category = item.Category,
                PublisherId = item.ID,
                Price = item.ID,
                PublicationYear = item.ID,


            }).ToList();
            return View(mapToVM);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Input is not valid";
                return View(model);
            }

            var book = new Book()
            {
                Title = model.Title,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Category = model.Category,
                Publisher = model.Publisher,
                Authors = model.Authors,
                CategoryId = model.CategoryId,
                Price = model.Price,
                isDeleted = false,
            };
            await _db.Books.AddAsync(book);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Is Working";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var bookCategory = await _db.BookCategories.FindAsync(id);
            if (bookCategory == null)
            {
                return NotFound();
            }
            var model = new BookCategoryEditVM()
            {
                ID = bookCategory.ID,
                Name = bookCategory.Name,
                Description = bookCategory.Description,
                Books = bookCategory.Books,
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookEditVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Input is not valid";
                return View(model);
            }
            var book = await _db.Books.FindAsync(model.ID);
            if (book == null)
            {
                return NotFound();
            }
			book.Publisher = model.Publisher;
			book.Authors = model.Authors;
			book.Title = model.Title;
            book.PublicationYear = model.PublicationYear;
            book.Price = model.Price;
            book.CategoryId = model.CategoryId;
            book.Category = model.Category;
            book.PublisherId = model.PublisherId;
            book.UpdatedDate = DateTime.Now;
            await _db.SaveChangesAsync();
            TempData["Success"] = "Datas successfully modified";

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var bookCategory = _db.Books.Find(id);
            if (bookCategory == null)
            {
                return NotFound();
            }
            var model = new BookVM()
            {
                ID = id,
                Publisher = bookCategory.Publisher,
                Authors = bookCategory.Authors,
                Title = bookCategory.Title,
                PublicationYear = bookCategory.PublicationYear,
                Price = bookCategory.Price,
                CategoryId = bookCategory.CategoryId,
                Category = bookCategory.Category,
                CreatedDate = bookCategory.CreatedDate
            };
            return View(model);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var book = _db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }
            var model = new BookVM()
            {
                Publisher = book.Publisher,
                Authors = book.Authors,
                Title = book.Title,
                PublicationYear = book.PublicationYear,
                Price = book.Price,
                CategoryId = book.CategoryId,
                Category = book.Category,
                PublisherId = book.PublisherId,
                ID = book.ID,
                CreatedDate = book.CreatedDate,
                UpdatedDate = book.UpdatedDate,

            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Book model)
        {
            var book = await _db.Books.FindAsync(model.ID);
            book.isDeleted = true;
            book.UpdatedDate = DateTime.Now;
            await _db.SaveChangesAsync();
            TempData["Success"] = "Row is successfully deleted !";

            return RedirectToAction(nameof(Index));
        }
    }
}
