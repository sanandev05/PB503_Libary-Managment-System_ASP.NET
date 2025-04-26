using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PB503_Libary_Managment_System_ASP.NET.Data;
using PB503_Libary_Managment_System_ASP.NET.Models;
using PB503_Libary_Managment_System_ASP.NET.View_Models.BookCategory;

namespace PB503_Libary_Managment_System_ASP.NET.Controllers
{
	public class BookCategoryController : Controller
	{
		private readonly LibaryDbContext _db;
        public BookCategoryController(LibaryDbContext db)
        {
			_db = db;  
        }
        public async Task<IActionResult> Index()
		{
			var getDatas= await _db.BookCategories.Where(item=>!item.isDeleted).ToListAsync();
			var mapToVM = getDatas.Select(item => new BookCategoryVM()
			{
				ID = item.ID,
				CreatedDate = item.CreatedDate,
				UpdatedDate = item.UpdatedDate,
				Name = item.Name,
				Books = item.Books,
				Description = item.Description,
			}).ToList();
			return View(mapToVM);
		}
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(BookCategoryCreateVM model)
		{
			if(!ModelState.IsValid)
			{
				return View(model);
			}

			var bookCategory = new BookCategory()
			{
				Name = model.Name,
				Description = model.Description,
				CreatedDate = DateTime.Now,
				UpdatedDate = DateTime.Now,
				Books = model.Books,
				isDeleted = false,
			};
			await _db.BookCategories.AddAsync(bookCategory);
			await _db.SaveChangesAsync();

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
		public async Task<IActionResult> Edit(BookCategoryEditVM model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var bookCategory = await _db.BookCategories.FindAsync(model.ID);
			if (bookCategory == null)
			{
				return NotFound();
			}
			bookCategory.Name = model.Name;
			bookCategory.Description = model.Description;
			bookCategory.UpdatedDate = DateTime.Now;
			bookCategory.Books = model.Books;
			await _db.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		[HttpGet]
		public IActionResult Details(int id)
		{
			var bookCategory = _db.BookCategories.Find(id);
			if (bookCategory == null)
			{
				return NotFound();
			}
			var model = new BookCategoryVM()
			{
				ID = bookCategory.ID,
				Name = bookCategory.Name,
				Description = bookCategory.Description,
				Books = bookCategory.Books,
				UpdatedDate = bookCategory.UpdatedDate,
				CreatedDate = bookCategory.CreatedDate,
			};
			return View(model);
		}
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var bookCategory = _db.BookCategories.Find(id);
            if (bookCategory == null)
            {
                return NotFound();
            }
            var model = new BookCategoryVM()
            {
                ID = bookCategory.ID,
                Name = bookCategory.Name,
                Description = bookCategory.Description,
                Books = bookCategory.Books,
                UpdatedDate = bookCategory.UpdatedDate,
                CreatedDate = bookCategory.CreatedDate,
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(BookCategoryVM categoryVM)
		{
			var bookCategory = await _db.BookCategories.FindAsync(categoryVM.ID);
			bookCategory.isDeleted = true;
            bookCategory.UpdatedDate = DateTime.Now;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
