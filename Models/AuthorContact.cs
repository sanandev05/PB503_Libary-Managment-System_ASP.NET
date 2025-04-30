using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PB503_Libary_Managment_System_ASP.NET.Models
{
	public class AuthorContact : BaseEntity
	{
		[StringLength(200)]
		public string Address { get; set; }

		[StringLength(50)]
		public string Phone { get; set; }

		[StringLength(100)]
		public string Email { get; set; }

		public int? AuthorId { get; set; }
		public Author? Author { get; set; }
	}
}
