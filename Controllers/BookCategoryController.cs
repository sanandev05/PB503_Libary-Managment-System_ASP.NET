using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PB503_Libary_Managment_System_ASP.NET.Data;
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
			var getDatas= await _db.BookCategories.ToListAsync();
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
	}
}
