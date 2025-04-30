using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PB503_Libary_Managment_System_ASP.NET.Data;
using PB503_Libary_Managment_System_ASP.NET.Models;
using PB503_Libary_Managment_System_ASP.NET.View_Models.AuthorVM;


namespace PB503_Libary_Managment_System_ASP.NET.Controllers
{
    public class AuthorController : Controller
    {
        private readonly LibaryDbContext _db;
        public AuthorController(LibaryDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var getDatas = await _db.Authors.Where(item => !item.isDeleted).ToListAsync();
            var mapToVM = getDatas.Select(item => new AuthorVM()
            {
                ID = item.ID,
                CreatedDate = item.CreatedDate,
                UpdatedDate = item.UpdatedDate,
                Books = item.Books,
                Contact = item.Contact,
                FullName = item.FullName,

            }).ToList();
            return View(mapToVM);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Input is not valid";
                return View(model);
            }

            var author = new Author()
            {
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Contact = model.Contact,
                Books = model.Books,
                FullName = model.FullName
            };
            await _db.Authors.AddAsync(author);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Is Working";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var author = await _db.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            var model = new AuthorEditVM()
            {
                ID = author.ID,
                FullName = author.FullName,
                Books = author.Books,
                Contact = author.Contact,

            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AuthorEditVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Input is not valid";
                return View(model);
            }
            var bookCategory = await _db.Authors.FindAsync(model.ID);
            if (bookCategory == null)
            {
                return NotFound();
            }
            
            bookCategory.UpdatedDate = DateTime.Now;
            bookCategory.FullName = model.FullName;
            bookCategory.Contact = model.Contact;
            bookCategory.Books = model.Books;
            bookCategory.isDeleted = false;
            await _db.SaveChangesAsync();
            TempData["Success"] = "Datas successfully modified";

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var author = _db.Authors.Find(id);
            if (author == null)
            {
                return NotFound();
            }
            var model = new AuthorVM()
            {
                ID = id,
                CreatedDate = author.CreatedDate,
                Books = author.Books,
                Contact = author.Contact,
                FullName = author.FullName,
                UpdatedDate = author.UpdatedDate

            };
            return View(model);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var author = _db.Authors.Find(id);
            if (author == null)
            {
                return NotFound();
            }
            var model = new AuthorVM()
            {
                Books = author.Books,
                Contact = author.Contact,
                FullName = author.FullName,
                ID = id,
                CreatedDate = author.CreatedDate,
                UpdatedDate = DateTime.Now
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Author model)
        {
            var author = await _db.Authors.FindAsync(model.ID);
            author.isDeleted = true;
            author.UpdatedDate = DateTime.Now;
            await _db.SaveChangesAsync();
            TempData["Success"] = "Row is successfully deleted !";

            return RedirectToAction(nameof(Index));
        }
    }
}
