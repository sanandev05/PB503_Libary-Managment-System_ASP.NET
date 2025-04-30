using PB503_Libary_Managment_System_ASP.NET.Models;

namespace PB503_Libary_Managment_System_ASP.NET.View_Models.AuthorContactVM
{
    public class AuthorContactEditVM 
    {
        public int ID { get; set; }
        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
