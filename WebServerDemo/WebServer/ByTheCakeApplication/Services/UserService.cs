namespace WebServer.ByTheCakeApplication.Services
{
    using System;
    using System.Linq;
    using WebServer.ByTheCakeApplication.Data;
    using WebServer.ByTheCakeApplication.Data.Models;
    using WebServer.ByTheCakeApplication.ViewModels.Account;

    public class UserService : IUserService
    {
        public bool Create(string username, string password)
        {
            using (var db = new ByTheCakeDbContext())
            {
                if (db.Users.Any(u => u.Username == username))
                {
                    return false;
                }
                var user = new User(username, password, DateTime.UtcNow);

                db.Add(user);
                db.SaveChanges();

                return true;
            }
        }

        public bool Find(string username, string password)
        {
            using (var db = new ByTheCakeDbContext())
            {
                return db.Users.Any(u => u.Username == username && u.Password == password);
            }
        }

        public ProfileViewModel Profile(string username)
        {
            using (var db = new ByTheCakeDbContext())
            {
                return db
                    .Users
                    .Where(u => u.Username == username)
                    .Select(u => new ProfileViewModel
                    (
                        u.Username,
                        u.RegistrationDate,
                        u.Orders.Count()
                    ))
                    .FirstOrDefault();
            }
        }
    }
}
