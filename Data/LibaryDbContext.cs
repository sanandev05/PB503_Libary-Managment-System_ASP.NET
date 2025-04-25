using Microsoft.EntityFrameworkCore;
using PB503_Libary_Managment_System_ASP.NET.Models;
using PB503_Libary_Managment_System_ASP.NET.View_Models.BookCategory;

namespace PB503_Libary_Managment_System_ASP.NET.Data
{
	public class LibaryDbContext : DbContext
	{
        public LibaryDbContext(DbContextOptions<LibaryDbContext>options):base(options) { }
        
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthorContact> AuthorsContacts { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
    }
}
