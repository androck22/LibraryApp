
namespace LibraryApp.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Year { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }

        // Внешний ключ
        public int? UserId { get; set; }
        // Навигационное свойство
        public User User { get; set; }
    }
}
