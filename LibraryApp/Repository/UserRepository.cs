using LibraryApp.Data;
using LibraryApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryApp.Repository
{
    public static class UserRepository
    {
        //получить пользователя по идентификатору
        public static void GetUser(int id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var userName = from user in db.Users
                             where user.Id == id
                             select user;

                foreach (var user in userName)
                {
                    Console.WriteLine($"Пользователь {user.Name} имеет идентификатор {id}");
                    Console.WriteLine();
                }
            }
        }
        //получить всех пользователей
        public static List<User> GetAllUsers()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var result = db.Users.ToList();
                return result;
            }
        }
        //создание пользователя
        public static string CreateUser(User user)
        {
            string result = "Уже существует";

            using (ApplicationContext db = new ApplicationContext())
            {
                //проверяем существует ли данный пользователь
                bool checkIsExist = db.Users.Any(u => u.Name == user.Name && u.Email == user.Email);
                if (!checkIsExist)
                {
                    User newUser = new User { Name = user.Name, Email = user.Email };
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    result = "Пользователь " + user.Name + " добавлен!";
                }
            }
            return result;
        }
        //удаление пользователя
        public static string DeleteUser(User user)
        {
            string result = "Такого пользователя не существует";

            using (ApplicationContext db = new ApplicationContext())
            {
                bool checkIsExist = db.Users.Any(u => u.Name == user.Name && u.Email == user.Email);
                if (checkIsExist)
                {
                    db.Users.Remove(user);
                    db.SaveChanges();
                    result = "Пользователь " + user.Name + " удален!";
                }
            }
            return result;
        }
        //обновление имени пользователя
        public static string UpdateUser(User oldUser, string newName)
        {
            string result = "Такого пользователя не существует";

            using (ApplicationContext db = new ApplicationContext())
            {
                User user = db.Users.FirstOrDefault(user => user.Id == oldUser.Id);
                user.Name = newName;
                db.SaveChanges();
                result = "Имя пользователя " + user.Name + " обновлено!";
            }
            Console.WriteLine($"Пользователь {oldUser.Name} был изменен на {newName}");
            Console.WriteLine();

            return result;
        }
    }
}
