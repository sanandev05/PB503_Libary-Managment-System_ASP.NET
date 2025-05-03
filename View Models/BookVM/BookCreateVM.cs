using Microsoft.AspNetCore.Mvc.Rendering;
using PB503_Libary_Managment_System_ASP.NET.Models;
using System.ComponentModel.DataAnnotations;

namespace PB503_Libary_Managment_System_ASP.NET.View_Models.BookVM
{
    public class BookCreateVM
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Publication year is required.")]
        [Range(1900, 2025, ErrorMessage = "Publication year must be between 1900 and 2025.")]
        public int PublicationYear { get; set; }

        [Required(ErrorMessage = "Price is required.")]       
        public decimal Price { get; set; }


        public int PublisherId { get; set; }
        public int CategoryId { get; set; }

        public List<int>? AuthorIds { get; set; }

        public List<int>? SelectedAuthorIds { get; set; } = new List<int>();
        public List<Author> Authors { get; set; } = new List<Author>();
        public List<BookCategory>? BookCategories { get; set; }
        public Publisher? Publisher { get; set; }
    }
}
