using LibraryApp.Data;
using LibraryApp.Entities;
using LibraryApp.Repository;
using System;
using System.Collections.Generic;

namespace LibraryApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var user1 = new User { Name = "Arthur", Email = "arthur@gamil.com" };
                var user2 = new User { Name = "Klim", Email = "klim@gamil.com" };
                var user3 = new User { Name = "Oleg", Email = "oleg@gamil.com" };
                var user4 = new User { Name = "Andrew", Email = "andrew@gmail.com" };

                var book1 = new Book { Name = "Кот в сапогах", Year = 1697, Author = "Шарль Перро", Genre = "Сказка" };
                var book2 = new Book { Name = "Золотая рыбка", Year = 1835, Author = "Александр Сергеевич Пушкин", Genre = "Сказка" };
                var book3 = new Book { Name = "Колобок", Year = 1873, Author = "Народная", Genre = "Сказка" };
                var book4 = new Book { Name = "1984", Year = 1949, Author = "Джордж Оруэлл", Genre = "Роман-антиутопия" };
                var book5 = new Book { Name = "Алхимик", Year = 1988, Author = "Пауло Коэльо", Genre = "Роман" };
                var book6 = new Book { Name = "Чужак", Year = 2018, Author = "Стивен Кинг", Genre = "Детектив" };
                var book7 = new Book { Name = "451 градус по Фаренгейту", Year = 1953, Author = "Рэй Брэдбери", Genre = "Роман-антиутопия" };


                db.Users.AddRange(user1, user2, user3);
                db.SaveChanges();

                db.Books.AddRange(book1, book2, book4, book3, book5, book6, book7);
                db.SaveChanges();

                //создание пользователя
                UserRepository.CreateUser(user4);

                //удаление пользователя
                //UserRepository.DeleteUser(user4);

                //обновление имени пользователя
                UserRepository.UpdateUser(user1, "Andrew");

                //получить пользователя по идентификатору
                UserRepository.GetUser(3);

                //получить всех пользователей
                List<User> allUsers = UserRepository.GetAllUsers();
                Console.WriteLine("Все пользователи библиотеки: ");
                foreach (var user in allUsers)
                {
                    Console.WriteLine(user.Name);
                }
                Console.WriteLine();


                book1.User = user1;
                user2.Books.Add(book2);
                book3.User = user3;
                user4.Books.Add(book4);
                book5.User = user2;
                user2.Books.Add(book6);
                book7.User = user4;
                db.SaveChanges();

                //-------------------------------------------------------------------------------
                //дополнительные LINQ запросы

                //получение списка книг определенного жанра и вышедших между определенными годами
                #region
                var books = BookRepository.GetBooksByGenre("Сказка", 1800, 1950);
                Console.Write("Список книг указанного жанра, вышедших между определенными годами: " + "\n");
                foreach (var book in books)
                {
                    Console.Write(book.Name + ", \n");
                }
                Console.WriteLine();
                #endregion

                //получение количества книг определенного автора в библиотеке
                #region               
                var countBooksByAuthor = BookRepository.GetCountBooksByAuthor("Джордж Оруэлл");
                Console.Write("Количество книг указанного автора: ");
                Console.WriteLine(countBooksByAuthor);
                Console.WriteLine();
                #endregion

                //получение количества книг определенного автора в библиотеке
                #region                
                var countBooksByGenre = BookRepository.GetCountBooksByGenre("Роман-антиутопия");
                Console.Write("Количество книг указанного жанра: ");
                Console.WriteLine(countBooksByGenre);
                Console.WriteLine();
                #endregion

                //запрос, есть ли книга определенного автора и с определенным названием в библиотеке
                #region
                var bookOfCertainAuthorAndCertainTitle = BookRepository
                    .GetBookOfCertainAuthorAndCertainTitle("Шарль Перро", "Кот в сапогах");
                if (bookOfCertainAuthorAndCertainTitle)
                    Console.WriteLine("Данная книга есть в библиотеке");
                else
                    Console.WriteLine("Данная книги нет в библиотеке");
                Console.WriteLine();
                #endregion

                //запрос, есть ли определенная книга на руках у пользователя
                #region  
                var certainBookInTheUsersHands = BookRepository
                    .GetCertainBookInTheUsersHands("Алхимик", "Klim");
                if (certainBookInTheUsersHands)
                    Console.WriteLine("Данная книга на руках у указанного пользователя");
                else
                    Console.WriteLine("Данной книги нет на руках у указанного пользователя");
                Console.WriteLine();
                #endregion

                //запрос на получение количества книг на руках у пользователя
                #region 
                var numberOfBooksInTheUsersHands = BookRepository.GetNumberOfBooksInTheUsersHands(user2);
                Console.WriteLine($"У пользователя {user2.Name} на руках находится {numberOfBooksInTheUsersHands} книга(-ги)");
                Console.WriteLine();
                #endregion

                //запрос на получение последней вышедшей книги
                #region 
                string theLatestPublishedeBook = BookRepository.GetTheLatestPublishedeBook();
                Console.WriteLine($"Последняя изданная книга в библиотеке: {theLatestPublishedeBook}");
                Console.WriteLine();
                #endregion

                //запрос на получение списка всех книг, отсортированного в алфавитном порядке по названию
                #region 
                Console.WriteLine("Списка всех книг, отсортированного в алфавитном порядке по названию:");
                BookRepository.GetAllBooksList();
                #endregion

                //запрос на получение списка всех книг, отсортированного в порядке убывания года их выхода
                #region 
                Console.WriteLine("Списка всех книг, отсортированного в порядке убывания года их выхода:");
                BookRepository.GetAllBooksListByYearDescending();
                #endregion
            }

        }
    }
}
