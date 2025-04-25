using System.ComponentModel.DataAnnotations;

namespace PB503_Libary_Managment_System_ASP.NET.Models
{
	public class BookCategory : BaseEntity
	{
		[Required]
		[StringLength(100)]
		public string Name { get; set; }
		[Required]
		[StringLength(500)]
		public string Description { get; set; }
		public List<Book> Books { get; set; }
	}
}
