using System.ComponentModel.DataAnnotations;

namespace InstructorInfoSystem.Models
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "Instructor ID is required.")]
        [StringLength(30, ErrorMessage = "Instructor ID cannot exceed 30 characters.")]
        [Display(Name = "Instructor ID")]
        public string InstructorId { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50)]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50)]
        [Display(Name = "Last name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{12,}$",
            ErrorMessage = "Password must be at least 12 characters and include uppercase, lowercase, a number, and a special character.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
