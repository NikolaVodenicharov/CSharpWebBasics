namespace WebServer.GameStoreApplication.Common
{
    public class ValidationConstants
    {
        public const string InvalidMinLengthErrorMessage = "{0} must be at least {1} symbols.";
        public const string InvalidMaxLengthErrorMessage = "{0} can not be more {1} symbols.";
        public const string ExactLengthErrorMessage = "{0} must be exactly {1} symbols.";

        public class Account
        {
            public const int EmailMaxLength = 30;

            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 30;

            public const int NameMinLength = 2;
            public const int NameMaxLength = 30;
        }

        public class Game
        {
            public const int TitleMinLength = 3;
            public const int TitleMaxLength = 100;
            public const int VideoIdLength = 11;
            public const int DescriptionMinLength = 20;
        }
    }
}
