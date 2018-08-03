﻿namespace WebServer.GameStoreApplication.Data.Models
{
    using System.Collections.Generic;

    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public bool IsAdministrator { get; set; }
        public ICollection<UserGame> UsersGames { get; set; } = new List<UserGame>();
    }
}
