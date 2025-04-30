using PB503_Libary_Managment_System_ASP.NET.Models;

namespace PB503_Libary_Managment_System_ASP.NET.View_Models.BookCategoryVM
{
    public class BookCategoryCreateVM
    {
		public string Name { get; set; }
		public string Description { get; set; }
		public List<Book>? Books { get; set; }
	}
}
