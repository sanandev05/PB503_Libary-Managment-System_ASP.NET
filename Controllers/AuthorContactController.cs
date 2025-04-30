using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PB503_Libary_Managment_System_ASP.NET.Data;
using PB503_Libary_Managment_System_ASP.NET.Models;
using PB503_Libary_Managment_System_ASP.NET.View_Models.AuthorContactVM;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PB503_Libary_Managment_System_ASP.NET.Controllers
{
    public class AuthorContactController : Controller
    {
        private readonly LibaryDbContext _db;

        public AuthorContactController(LibaryDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var getDatas = await _db.AuthorsContacts.Where(x => !x.isDeleted).ToListAsync();
            var mapToVM = getDatas.Select(x => new AuthorContactVM
            {
                ID = x.ID,
                Phone = x.Phone,
                Email = x.Email,
                Address = x.Address,
                CreatedDate = x.CreatedDate,
                UpdatedDate = x.UpdatedDate
            }).ToList();

            return View(mapToVM);
        }

        public IActionResult Create()
        {
            ViewBag.AuthorID = new SelectList(_db.Authors.Where(item => !item.isDeleted).ToList(), "ID", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorContactCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Input is not valid";
                return View(model);
            }

            var contact = new AuthorContact
            {
                Phone = model.Phone,
                Email = model.Email,
                Address = model.Address,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                isDeleted = false
            };

            await _db.AuthorsContacts.AddAsync(contact);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Contact created successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var contact = await _db.AuthorsContacts.FindAsync(id);
            if (contact == null) return NotFound();

            var model = new AuthorContactEditVM
            {
                ID = contact.ID,
                Phone = contact.Phone,
                Email = contact.Email,
                Address = contact.Address
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AuthorContactEditVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Input is not valid";
                return View(model);
            }

            var contact = await _db.AuthorsContacts.FindAsync(model.ID);
            if (contact == null) return NotFound();

            contact.Phone = model.Phone;
            contact.Email = model.Email;
            contact.Address = model.Address;
            contact.UpdatedDate = DateTime.Now;

            await _db.SaveChangesAsync();
            TempData["Success"] = "Contact updated successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var contact = _db.AuthorsContacts.Find(id);
            if (contact == null) return NotFound();

            var model = new AuthorContactVM
            {
                ID = contact.ID,
                Phone = contact.Phone,
                Email = contact.Email,
                Address = contact.Address,
                CreatedDate = contact.CreatedDate,
                UpdatedDate = contact.UpdatedDate
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var contact = _db.AuthorsContacts.Find(id);
            if (contact == null) return NotFound();

            var model = new AuthorContactVM
            {
                ID = contact.ID,
                Phone = contact.Phone,
                Email = contact.Email,
                Address = contact.Address,
                CreatedDate = contact.CreatedDate,
                UpdatedDate = contact.UpdatedDate
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(AuthorContactVM model)
        {
            var contact = await _db.AuthorsContacts.FindAsync(model.ID);
            if (contact == null) return NotFound();

            contact.isDeleted = true;
            contact.UpdatedDate = DateTime.Now;

            await _db.SaveChangesAsync();
            TempData["Success"] = "Contact deleted successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
