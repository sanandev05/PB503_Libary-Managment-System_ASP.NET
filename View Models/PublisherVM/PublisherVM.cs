using PB503_Libary_Managment_System_ASP.NET.Models;
using System.ComponentModel.DataAnnotations;

namespace PB503_Libary_Managment_System_ASP.NET.View_Models.PublisherVM
{
    public class PublisherVM : BaseEntityVM
    {
        
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public List<Book> Books { get; set; }
    }
}
