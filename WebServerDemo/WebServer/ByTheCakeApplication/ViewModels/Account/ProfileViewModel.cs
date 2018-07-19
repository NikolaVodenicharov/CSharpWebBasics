namespace WebServer.ByTheCakeApplication.ViewModels.Account
{
    using System;

    public class ProfileViewModel
    {
        public ProfileViewModel(string username, DateTime registrationDate, int totalOrders)
        {
            this.Username = username;
            this.RegistrationDate = registrationDate;
            this.TotalOrders = totalOrders;
        }

        public string Username { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int TotalOrders { get; set; }
    }
}
