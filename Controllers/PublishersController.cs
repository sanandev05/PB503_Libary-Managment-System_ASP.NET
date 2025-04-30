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
using PB503_Libary_Managment_System_ASP.NET.View_Models.PublisherVM;

namespace PB503_Libary_Managment_System_ASP.NET.Controllers
{
    public class PublishersController : Controller
    {
        private readonly LibaryDbContext _db;
        public PublishersController(LibaryDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var getDatas = await _db.Publishers.Where(item => !item.isDeleted).ToListAsync();
            var mapToVM = getDatas.Select(item => new PublisherVM()
            {
                ID = item.ID,
                CreatedDate = item.CreatedDate,
                UpdatedDate = item.UpdatedDate,
                Name = item.Name,
                Books = item.Books,
                Address = item.Address,
                Email = item.Email,
                Phone = item.Phone,
            }).ToList();
            return View(mapToVM);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PublisherCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Input is not valid";
                return View(model);
            }

            var publisher = new Publisher()
            {
                Name = model.Name,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Phone = model.Phone,
                Email = model.Email,
                Address = model.Address,
                ID = model.ID,
                isDeleted = false,
                
            };
            await _db.Publishers.AddAsync(publisher);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Is Working";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var bookCategory = await _db.Publishers.FindAsync(id);
            if (bookCategory == null)
            {
                return NotFound();
            }
            var model = new PublisherEditVM()
            {
               ID = bookCategory.ID,
                Name = bookCategory.Name,
                Address = bookCategory.Address,
                Phone = bookCategory.Phone,
                Email = bookCategory.Email,
                Books = bookCategory.Books,
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Publisher model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Input is not valid";
                return View(model);
            }
            var publisher = await _db.Publishers.FindAsync(model.ID);
            if (publisher == null)
            {
                return NotFound();
            }
            publisher.Name = model.Name;
            publisher.UpdatedDate = DateTime.Now;
            publisher.Books = model.Books;
            publisher.Address = model.Address;
            publisher.Phone = model.Phone;
            publisher.Email = model.Email;
            publisher.isDeleted = false;

            await _db.SaveChangesAsync();
            TempData["Success"] = "Datas successfully modified";

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var publisher = _db.Publishers.Find(id);
            if (publisher   == null)
            {
                return NotFound();
            }
            var model = new Publisher()
            {
                ID = publisher.ID,
                Name = publisher.Name,
                Books = publisher.Books,
                UpdatedDate = publisher.UpdatedDate,
                CreatedDate = publisher.CreatedDate,
                Address = publisher.Address,
                Email = publisher.Email,
                Phone = publisher.Phone,
                isDeleted = publisher.isDeleted
            };
            return View(model);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var publisher = _db.Publishers.Find(id);
            if (publisher == null)
            {
                return NotFound();
            }
            var model = new Publisher()
            {
               isDeleted = publisher.isDeleted,
                ID = publisher.ID,
                Name = publisher.Name,
                Books = publisher.Books,
                UpdatedDate = DateTime.Now,
                CreatedDate = publisher.CreatedDate,
                Address = publisher.Address,
                Email = publisher.Email,
                Phone = publisher.Phone
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Publisher model)
        {
            var publisher = await _db.Publishers.FindAsync(model.ID);
            model.isDeleted = true;
            model.UpdatedDate = DateTime.Now;
            await _db.SaveChangesAsync();
            TempData["Success"] = "Row is successfully deleted !";

            return RedirectToAction(nameof(Index));
        }
    }
}
