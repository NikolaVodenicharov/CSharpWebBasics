using System.ComponentModel.DataAnnotations;
using WebServer.GameStoreApplication.Common;
using WebServer.GameStoreApplication.Utilities;

namespace WebServer.GameStoreApplication.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Display(Name = "E-mail")]
        [Required]
        [EmailAddress]
        [MaxLength(ValidationConstants.Account.EmailMaxLength,
            ErrorMessage = ValidationConstants.InvalidMaxLengthErrorMessage)]
        public string Email { get; set; }

        [Display(Name = "Full name")]
        [MinLength(
            ValidationConstants.Account.NameMinLength,
            ErrorMessage = ValidationConstants.InvalidMinLengthErrorMessage)]
        [MaxLength(ValidationConstants.Account.NameMaxLength,
            ErrorMessage = ValidationConstants.InvalidMaxLengthErrorMessage)]
        public string FullName { get; set; }

        [Required]
        [MinLength(
            ValidationConstants.Account.PasswordMinLength,
            ErrorMessage = ValidationConstants.InvalidMinLengthErrorMessage)]
        [MaxLength(
            ValidationConstants.Account.PasswordMaxLength,
            ErrorMessage = ValidationConstants.InvalidMaxLengthErrorMessage)]
        [Password]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
