namespace WebServer.ByTheCakeApplication.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User(string username, string password, DateTime registrationDate)
        {
            this.Username = username;
            this.Password = password;
            this.RegistrationDate = registrationDate;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public DateTime RegistrationDate { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
