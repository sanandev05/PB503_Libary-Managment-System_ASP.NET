using System.ComponentModel.DataAnnotations;

namespace PB503_Libary_Managment_System_ASP.NET.Models
{
	public class Author : BaseEntity
	{
		[Required]
		[StringLength(200)]
		public string FullName { get; set; }

    

        public  AuthorContact Contact { get; set; }
		public  List<Book>? Books { get; set; }
	}
}
