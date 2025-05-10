using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var getDatas = await _db.Authors.Where(item => !item.isDeleted).Include(x => x.Books).Include(x => x.Contact).ToListAsync();
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
            ViewBag.BookIDs = new SelectList(_db.Books.Where(item => !item.isDeleted).ToList(), "ID", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorCreateVM model)
        {
            ViewBag.BookIDs = new SelectList(_db.Books.Where(item => !item.isDeleted).ToList(), "ID", "Title");

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Input is not valid";
                return View(model);
            }

         
            var author = new Author()
            {
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
               // Contact = model.Contact,
                //Books = model.Books,
                FullName = model.FullName,
                //isDeleted = false,
                //ID =(int) contact.AuthorId,
            };
            await _db.Authors.AddAsync(author);
            await _db.SaveChangesAsync();
            AuthorContact contact = new AuthorContact()
            {

                Phone = model.ContactCreateVM.Phone,
                //ID = model.ID+10,
                Address = model.ContactCreateVM.Address,
                // Author = model.ContactCreateVM.Author,
                AuthorId = author.ID,
                Email = model.ContactCreateVM.Email,


            };

         //   Console.WriteLine("Author ID: " + author.ID);
            await _db.AuthorsContacts.AddAsync(contact);
            await _db.SaveChangesAsync();
          
            TempData["Success"] = "Is Working";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var author =  _db.Authors.Where(item => !item.isDeleted && item.ID==id).Include(x => x.Books).Include(x => x.Contact).ToList().FirstOrDefault();
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
            var author = await _db.Authors.FindAsync(model.ID);
            if (author == null)
            {
                return NotFound();
            }

            var contact=_db.AuthorsContacts.Where(item => !item.isDeleted && item.AuthorId == author.ID).Include(x=>x.Author).ToList().FirstOrDefault();
            if (contact == null)
            {
                return NotFound();
            }
            contact.Phone = model.Contact.Phone;
            contact.Address = model.Contact.Address;
            contact.Email = model.Contact.Email;
            contact.UpdatedDate = DateTime.Now;
            contact.isDeleted = false;


            author.UpdatedDate = DateTime.Now;
            author.FullName = model.FullName;
            author.Contact = contact;          
            
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
