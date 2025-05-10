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
		private readonly IWebHostEnvironment _env;
		public BookController(LibaryDbContext db, IWebHostEnvironment env)
		{
			_db = db;
			_env = env;
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
				Price = item.Price,
				PublicationYear = item.ID,
				Publisher = item.Publisher,
				AuthorIds = item.Authors.Select(x => x.ID).ToList(),
				CategoryId = item.CategoryId,
				PublisherId = item.PublisherId,
				ImageURL = item.ImageURL,
				

			}).ToList();
			return View(mapToVM);
		}
		public IActionResult Create()
		{
			ViewBag.CategoryId = new SelectList(_db.BookCategories.Where(x => !x.isDeleted).ToList(), "ID", "Description");
			ViewBag.AuthorIds = new MultiSelectList(_db.Authors.Where(x => !x.isDeleted).ToList(), "ID", "FullName");
			ViewBag.PublisherId = new SelectList(_db.Publishers.Where(x => !x.isDeleted).ToList(), "ID", "Name");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(BookCreateVM model)
		{

			if (!ModelState.IsValid)
			{
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
			if (model.ImageFile.Length > 100 * 1024)
			{
				ModelState.AddModelError("ImageFile", "Image size must be less than 100KB.");
			}
			if (model.ImageFile != null)
			{
				var fileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
				var path = Path.Combine(_env.WebRootPath, "images", "books", fileName);

				if(!Directory.Exists(Path.Combine(_env.WebRootPath, "images", "books")))
				{
					Directory.CreateDirectory(Path.Combine(_env.WebRootPath, "images", "books"));
				}

				using (var stream = new FileStream(path, FileMode.Create))
				{
					await model.ImageFile.CopyToAsync(stream);
				}
				model.ImageURL = "/images/books/" + fileName;
			}


			var authors = _db.Authors
			.Where(a => model.SelectedAuthorIds.Contains(a.ID) && !a.isDeleted)
			.ToList();

			var books = new Book()
			{
				Title = model.Title,
				PublicationYear = model.PublicationYear,
				Price = model.Price,
				PublisherId = model.PublisherId,
				CategoryId = model.CategoryId,
				CreatedDate = DateTime.Now,
				UpdatedDate = DateTime.Now,
				Authors = authors,
				Category = await _db.BookCategories.FirstOrDefaultAsync(x => x.ID == model.CategoryId && !x.isDeleted),
				ImageURL	=model.ImageURL,
				
			};

			await _db.Books.AddAsync(books);
			await _db.SaveChangesAsync();
			TempData["Success"] = "Is Working";

			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var book = await _db.Books
				.Include(x => x.Authors)
				.Include(x => x.Publisher)
				.Include(x => x.Category)
				.FirstOrDefaultAsync(x => x.ID == id && !x.isDeleted);
			if (book == null)
			{
				NotFound();
			}
			var model = new BookEditVM()
			{
				ID = book.ID,
				Title = book.Title,
				PublicationYear = book.PublicationYear,
				Price = book.Price,
				CategoryId = book.CategoryId,
				PublisherId = book.PublisherId,
				AuthorIds = book.Authors.Select(x => x.ID).ToList(),
				Authors = book.Authors,
			};
			ViewBag.CategoryId = new SelectList(_db.BookCategories.Where(x => !x.isDeleted), "ID", "Description");
			ViewBag.PublisherId = new SelectList(_db.Publishers.Where(x => !x.isDeleted), "ID", "Name");
			ViewBag.AuthorIds = new MultiSelectList(_db.Authors.Where(x => !x.isDeleted), "ID", "FullName");

			return View(model);
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
			var book = await _db.Books.Include(x => x.Authors).Include(y => y.Publisher).Include(z => z.Category).FirstOrDefaultAsync(x => x.ID == model.ID);
			if (book == null)
			{
				return NotFound();
			}
			var selectedAuthors = await _db.Authors
			   .Where(a => model.AuthorIds.Contains(a.ID) && !a.isDeleted)
			   .ToListAsync();
			book.Authors.Clear();
			book.Publisher = await _db.Publishers.FirstOrDefaultAsync(x => x.ID == model.PublisherId && !x.isDeleted);
			book.Category = await _db.BookCategories.FirstOrDefaultAsync(x => x.ID == model.CategoryId && !x.isDeleted);
			foreach (var author in selectedAuthors)
			{
				book.Authors.Add(author);
			}

			book.Title = model.Title;
			book.PublicationYear = model.PublicationYear;
			book.Price = model.Price;
			book.PublisherId = model.PublisherId;
			book.CategoryId = model.CategoryId;
			book.UpdatedDate = DateTime.Now;



			await _db.SaveChangesAsync();
			TempData["Success"] = "Datas successfully modified";

			return RedirectToAction(nameof(Index));
		}
		[HttpGet]
		public IActionResult Details(int id)
		{
			var bookCategory = _db.Books.Where(x => x.ID == id && !x.isDeleted)
				.Include(x => x.Authors)
				.Include(x => x.Publisher)
				.Include(x => x.Category)
				.FirstOrDefault();
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
				CreatedDate = bookCategory.CreatedDate,
				UpdatedDate = bookCategory.UpdatedDate,
				AuthorIds = bookCategory.Authors.Select(x => x.ID).ToList(),
				PublisherId = bookCategory.PublisherId,
			};
			return View(model);
		}
		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			var book = await _db.Books.Include(x => x.Authors).Include(y => y.Publisher).Include(z => z.Category).FirstOrDefaultAsync(x => x.ID == id);
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
