using LibraryApp.Data;
using LibraryApp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryApp.Repository
{
    public static class BookRepository
    {
        //получить книгу по идентификатору
        public static void GetBook(int id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var bookName = from book in db.Books
                               where book.Id == id
                               select book;

                foreach (var book in bookName)
                {
                    Console.WriteLine($"Книга {book.Name} имеет идентификатор {id}");
                    Console.WriteLine();
                }
            }
        }

        //получить все книги
        public static List<Book> GetAllBooks()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Books.ToList();
                return result;
            }
        }

        //создание книги
        public static string CreateBook(Book book)
        {
            string result = "Уже существует";

            using (ApplicationContext db = new ApplicationContext())
            {
                //проверяем существует ли данная книга
                bool checkIsExist = db.Books.Any(b => b.Name == book.Name && b.Year == book.Year && b.Author == book.Author && b.Genre == book.Genre);
                if (!checkIsExist)
                {
                    Book newBook = new Book { Name = book.Name, Year = book.Year, Author = book.Author, Genre = book.Genre };
                    db.Books.Add(newBook);
                    db.SaveChanges();
                    result = "Книга " + book.Name + " добавлена!";
                }  
            }
            return result;
        }

        //удаление книги
        public static string DeleteBook(Book book)
        {
            string result = "Такой книги не существует";

            using (ApplicationContext db = new ApplicationContext())
            {
                db.Books.Remove(book);
                db.SaveChanges();
                result = "Книга " + book.Name + " удалена!";
            }
            return result;
        }

        //обновление года написания книги
        public static string UpdateBook(Book oldBook, int newYear)
        {
            string result = "Такой книги не существует";

            using (ApplicationContext db = new ApplicationContext())
            {
                Book book = db.Books.FirstOrDefault(book => book.Id == oldBook.Id);
                book.Year = newYear;
                db.SaveChanges();
                result = "Год написания книги " + oldBook.Name + " обновлен!";
            }
            return result;
        }


        //-------------------------------------------------------------------------------


        //дополнительные LINQ запросы

        //получение списка книг определенного жанра и вышедших между определенными годами
        public static List<Book> GetBooksByGenre(string genre, int year1, int year2)
        {
            var result = new List<Book>();
            using (ApplicationContext db = new ApplicationContext())
            {
                var list = db.Books.Where(b => b.Genre == genre && b.Year > year1 && b.Year < year2);
                result = list.ToList();

            }
            return result;
        }

        //получение количества книг определенного автора в библиотеке
        public static int GetCountBooksByAuthor(string author)
        {
            var count = 0;
            using (ApplicationContext db = new ApplicationContext())
            {
                count = db.Books.Where(b => b.Author == author).Count();
            }

            return count;
        }

        //получение количества книг определенного жанра в библиотеке
        public static int GetCountBooksByGenre(string genre)
        {
            var count = 0;
            using (ApplicationContext db = new ApplicationContext())
            {
                count = db.Books.Where(b => b.Genre == genre).Count();
            }

            return count;
        }

        //запрос, есть ли книга определенного автора и с определенным названием в библиотеке
        public static bool GetBookOfCertainAuthorAndCertainTitle(string author, string title)
        {
            bool book = false;
            using (ApplicationContext db = new ApplicationContext())
            {
                book = db.Books.Any(b => b.Author == author && b.Name == title);
            }

            return book;
        }

        //запрос, есть ли определенная книга на руках у пользователя
        public static bool GetCertainBookInTheUsersHands(string bookName, string userName)
        {
            bool user = false;
            using (ApplicationContext db = new ApplicationContext())
            {
                user = db.Books.Join(db.Users, b => b.UserId, u => u.Id, (b, u) 
                    => new { BookName = b.Name, UserName = u.Name })
                    .Any(b => b.BookName == bookName && b.UserName == userName);
            }

            return user;
        }

        //запрос на получение количества книг на руках у пользователя
        public static int GetNumberOfBooksInTheUsersHands(User user)
        {
            int count = 0;
            using(ApplicationContext db = new ApplicationContext())
            {
                count = db.Books.Where(u => u.UserId == user.Id).Select(b => b.Id).Count();                    
            }
            return count;
        }

        //запрос на получение последней вышедшей книги
        public static string GetTheLatestPublishedeBook()
        {
            string book;
            using (ApplicationContext db = new ApplicationContext())
            {
                book = db.Books.OrderByDescending(b => b.Year).Select(b => b.Name).FirstOrDefault();
            }
            
            return book;
        }

        //запрос на получение списка всех книг, отсортированного в алфавитном порядке по названию
        public static void GetAllBooksList()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var books = db.Books.OrderBy(b => b.Name).Select(b => b.Name).ToList();

                foreach (var book in books)
                {
                    Console.WriteLine(book);
                }
            }
            Console.WriteLine();
        }

        //запрос на получение списка всех книг, отсортированного в порядке убывания года их выхода
        public static void GetAllBooksListByYearDescending()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var books = db.Books.OrderByDescending(b => b.Year).Select(b => b.Name).ToList();

                foreach (var book in books)
                {
                    Console.WriteLine(book);
                }
            }
            Console.WriteLine();
        }
    }
}
