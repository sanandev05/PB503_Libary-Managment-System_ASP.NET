using PB503_Libary_Managment_System_ASP.NET.Models;

namespace PB503_Libary_Managment_System_ASP.NET.View_Models.BookVM
{
    public class BookVM : BaseEntityVM
    {
        public string Title { get; set; }
 
        public int PublicationYear { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public BookCategory Category { get; set; }

        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        public List<Author> Authors { get; set; }
    }
}
