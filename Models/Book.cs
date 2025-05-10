using System.ComponentModel.DataAnnotations;

namespace PB503_Libary_Managment_System_ASP.NET.Models
{
	public class Book : BaseEntity
	{

		[Required]
		[StringLength(200)]
		public string Title { get; set; }
		[Required]
		public int PublicationYear { get; set; }
		[Required]
		public decimal Price { get; set; }

		public int CategoryId { get; set; }
		public  BookCategory Category { get; set; }

		public int PublisherId { get; set; }
		public  Publisher Publisher { get; set; }

		public  List<Author> Authors { get; set; }
		public string? ImageURL { get; set; }
    }
}
