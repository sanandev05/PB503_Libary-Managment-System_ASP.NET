using Microsoft.AspNetCore.Mvc.Rendering;
using PB503_Libary_Managment_System_ASP.NET.Models;
using System.ComponentModel.DataAnnotations;

namespace PB503_Libary_Managment_System_ASP.NET.View_Models.BookVM
{
    public class BookVM : BaseEntityVM
    {
		[Required(ErrorMessage = "Title is required.")]
		public string Title { get; set; }

		[Required(ErrorMessage = "Publication year is required.")]
		public int PublicationYear { get; set; }

		[Required(ErrorMessage = "Price is required.")]

		public decimal Price { get; set; }

		[Required(ErrorMessage = "Category is required.")]
		public int CategoryId { get; set; }
		public BookCategory Category { get; set; }

		[Required(ErrorMessage = "Publisher is required.")]
		public int PublisherId { get; set; }
		public Publisher Publisher { get; set; }

		[Required(ErrorMessage = "At least one author must be selected.")]
		public List<int> AuthorIds { get; set; }
		public List<Author> Authors { get; set; }
	}
}
