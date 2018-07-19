namespace WebServer.ByTheCakeApplication.ViewModels.Account
{
    public class LoginViewModel
    {
        public LoginViewModel(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}
