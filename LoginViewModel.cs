using System.ComponentModel.DataAnnotations;

namespace InstructorInfoSystem.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email or Instructor ID is required.")]
        [Display(Name = "Email or Instructor ID")]
        public string EmailOrId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
