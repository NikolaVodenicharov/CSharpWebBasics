namespace WebServer.ByTheCakeApplication.ViewModels.Account
{
    using System;

    public class RegisterUserViewModel
    {
        public RegisterUserViewModel(string username, string password, string confirmPassword)
        {
            this.Username = username;
            this.Password = password;
            this.ConfirmPassword = confirmPassword;
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
