namespace WebServer.GameStoreApplication.Common
{
    public class Authentication
    {
        public Authentication(bool isAuthenticated = false, bool isAdmin = false)
        {
            this.IsAuthenticated = isAuthenticated;
            this.IsAdmin = isAdmin;
        }

        public bool IsAuthenticated { get; private set; }

        public bool IsAdmin { get; private set; }
    }
}
