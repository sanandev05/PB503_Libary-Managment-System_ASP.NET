using PB503_Libary_Managment_System_ASP.NET.Models;

namespace PB503_Libary_Managment_System_ASP.NET.View_Models.AuthorVM
{
    public class AuthorVM : BaseEntityVM
    {
        public string FullName { get; set; }

        public AuthorContact Contact { get; set; }
        public List<Book> Books { get; set; }

    }
}
