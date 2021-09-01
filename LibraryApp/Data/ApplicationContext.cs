using LibraryApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Data
{
    public class ApplicationContext : DbContext
    {
        // Объекты таблицы Users
        public DbSet<User> Users { get; set; }

        // Объекты таблицы Books
        public DbSet<Book> Books { get; set; }

        //public ApplicationContext()
        //{
        //    Database.EnsureDeleted();
        //    Database.EnsureCreated();
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=.\SQLEXPRESS;Database=LibraryDB;Trusted_Connection=True;");
        }
    }
}
