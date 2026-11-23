
using System.ComponentModel.DataAnnotations;

namespace InstructorInfoSystem.Models
{
    public class Instructor
    {
        [Key]
        [Required(ErrorMessage = "Instructor ID is required.")]
        public string InstructorId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; } = string.Empty;

        [Range(1, 120, ErrorMessage = "Please enter a valid age.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Designation is required.")]
        public string Designation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact Number is required.")]
        public string ContactNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Majorship is required.")]
        public string Majorship { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bachelor Degree is required.")]
        public string BachelorDegree { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;
    }
}
