namespace WebServer.GameStoreApplication.Utilities
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class PasswordAttribute : ValidationAttribute
    {
        public PasswordAttribute()
        {
            this.ErrorMessage = "Password should be at least 6 symbols long. Should have at least 1 upper case letter, 1 lower case and 1 digit.";
        }

        public override bool IsValid(object value)
        {
            var password = value as string;
            if (string.IsNullOrEmpty(password))
            {
                return true;
            }

            return
                password.Any(s => char.IsUpper(s)) &&
                password.Any(s => char.IsLower(s)) &&
                password.Any(s => char.IsDigit(s));
        }
    }
}
