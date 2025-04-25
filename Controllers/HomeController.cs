using Microsoft.AspNetCore.Mvc;
using PB503_Libary_Managment_System_ASP.NET.Models;
using System.Diagnostics;

namespace PB503_Libary_Managment_System_ASP.NET.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}
		public IActionResult RedirectToBookCategory()
		{
			return RedirectToAction("Index","BookCategory");
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
