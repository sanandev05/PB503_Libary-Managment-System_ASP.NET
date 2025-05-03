using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PB503_Libary_Managment_System_ASP.NET.Data;
using PB503_Libary_Managment_System_ASP.NET.Models;
using PB503_Libary_Managment_System_ASP.NET.View_Models.BookCategoryVM;
using PB503_Libary_Managment_System_ASP.NET.View_Models.BookVM;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            var getDatas = await _db.Books.Where(item => !item.isDeleted).Include(x => x.Authors)
            .Include(x => x.Publisher)
            .Include(x => x.Category)
            .ToListAsync();
            var mapToVM = getDatas.Select(item => new BookVM()
            {
                ID = item.ID,
                CreatedDate = item.CreatedDate,
                UpdatedDate = item.UpdatedDate,
                Title = item.Title,
                Authors = item.Authors,
                Category = item.Category,
                Price = item.ID,
                PublicationYear = item.ID,
                Publisher = item.Publisher,
                AuthorIds = item.Authors.Select(x => x.ID).ToList(),
                CategoryId = item.CategoryId,
                PublisherId = item.PublisherId,
            }).ToList();
            return View(mapToVM);
        }
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_db.BookCategories.Where(x=>!x.isDeleted).ToList(), "ID", "Description");
            ViewBag.AuthorIds = new  MultiSelectList(_db.Authors.Where(x=>!x.isDeleted).ToList(), "ID", "FullName");
            ViewBag.PublisherId = new SelectList(_db.Publishers.Where(x=>!x.isDeleted).ToList(), "ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( BookCreateVM model)
        {
           
            if (ModelState.IsValid)
            {
                var authors = new List<Author>();
                foreach (var item in _db.Authors.AsNoTracking().Where(x => !x.isDeleted).ToList())
                {
                    foreach (var item2 in model.SelectedAuthorIds)
                    {
                        if (item.ID == item2)
                        {
                            authors.Add(item);
                        }
                    }
                }
                var books = new Book()
                {
                    Title = model.Title,
                    PublicationYear = model.PublicationYear,
                    Price = model.Price,
                    PublisherId = model.PublisherId,
                    CategoryId = model.CategoryId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    Publisher = model.Publisher,
                    ID = model.ID,
                    Authors =authors,
                    Category = _db.BookCategories.Where(x=>!x.isDeleted).FirstOrDefault(x=>x.ID==model.CategoryId),
                    
                };

                foreach (var authorId in model.SelectedAuthorIds)
                {
                    books.Authors.Add(new Author
                    {
                        ID = authorId,
                        Books = new List<Book> { books }
                    });
                }
      
                _db.Add(books);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Is Working";

                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryId = new SelectList(_db.BookCategories.Where(x => !x.isDeleted).ToList(), "ID", "Description");
            ViewBag.AuthorIds = new MultiSelectList(_db.Authors.Where(x => !x.isDeleted).ToList(), "ID", "FullName");
            ViewBag.PublisherId = new SelectList(_db.Publishers.Where(x => !x.isDeleted).ToList(), "ID", "Name");

            var book = new Book()
            {
                Title = model.Title,
                PublicationYear = model.PublicationYear,
                Price = model.Price,
                CategoryId = model.CategoryId,
                PublisherId = model.PublisherId,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };
           
            
            TempData["Error"] = "Wrong Input";

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.CategoryId = new SelectList(_db.BookCategories.Where(x => !x.isDeleted ), "ID", "Description");
            ViewBag.PublisherId = new SelectList(_db.Publishers.Where(x => !x.isDeleted ), "ID", "Name");
            ViewBag.AuthorIds = new MultiSelectList(_db.Authors.Where(x => !x.isDeleted ), "ID", "FullName");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookEditVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Input is not valid";
                ViewBag.CategoryId = new SelectList(_db.BookCategories.Where(x => !x.isDeleted), "ID", "Description");
                ViewBag.PublisherId = new SelectList(_db.Publishers.Where(x => !x.isDeleted), "ID", "Name");
                ViewBag.AuthorIds = new MultiSelectList(_db.Authors.Where(x => !x.isDeleted), "ID", "FullName");
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
