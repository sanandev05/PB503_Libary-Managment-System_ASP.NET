using PB503_Libary_Managment_System_ASP.NET.Models;
using PB503_Libary_Managment_System_ASP.NET.View_Models.AuthorContactVM;

namespace PB503_Libary_Managment_System_ASP.NET.View_Models.AuthorVM
{
    public class AuthorCreateVM 
    {
        public int ID { get; set; }
        public string FullName { get; set; }

		public int ContactID { get; set; }
		public AuthorContact? Contact { get; set; }

        public List<int>? BookIds { get; set; }
        public List<Book>? Books { get; set; }
        public AuthorContactCreateVM ContactCreateVM { get; set; }
    }
}
