namespace WebServer.GameStoreApplication.Services
{
    using System.Linq;
    using WebServer.GameStoreApplication.Data;
    using WebServer.GameStoreApplication.Data.Models;
    using WebServer.GameStoreApplication.Services.Contracts;

    public class UserService : IUserService
    {
        public bool Create(string email, string name, string password)
        {
            using (var db = new GameStoreDbContext())
            {
                if (db.Users.Any(u => u.Email.Equals(email)))
                {
                    return false;
                }

                var isAdmin = !db.Users.Any();

                var user = new User
                {
                    Email = email,
                    FullName = name,
                    Password = password,
                    IsAdministrator = isAdmin
                };

                db.Users.Add(user);
                db.SaveChanges();
            }

            return true;
        }

        public bool Find(string email, string password)
        {
            using (var db = new GameStoreDbContext())
            {
                return db.Users.Any(u => u.Email.Equals(email) && u.Password.Equals(password));
            }
        }

        public bool IsAdmin(string email)
        {
            using (var db = new GameStoreDbContext())
            {
                return db.Users.Any(u => u.Email == email && u.IsAdministrator);
            }
        }
    }
}
