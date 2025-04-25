using System.ComponentModel.DataAnnotations;

namespace PB503_Libary_Managment_System_ASP.NET.Models
{
	public class Publisher : BaseEntity
	{
		[Required]
		[StringLength(200)]
		public string Name { get; set; }

		[StringLength(200)]
		public string Address { get; set; }

		[StringLength(50)]
		public string Phone { get; set; }

		[StringLength(100)]
		public string Email { get; set; }

		public List<Book> Books { get; set; }
	}
}
