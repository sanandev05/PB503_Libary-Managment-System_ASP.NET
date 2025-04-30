using PB503_Libary_Managment_System_ASP.NET.Models;

namespace PB503_Libary_Managment_System_ASP.NET.View_Models.AuthorVM
{
    public class AuthorCreateVM 
    {
        public int ID { get; set; }
        public string FullName { get; set; }

		public int ContactID { get; set; }
		public AuthorContact Contact { get; set; }
        public int BookID { get; set; }

		public List<Book> Books { get; set; }
    }
}
