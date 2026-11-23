using System.ComponentModel.DataAnnotations;

namespace InstructorInfoSystem.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "Subject code is required.")]
        [Display(Name = "Subject Code")]
        public string SubjectCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subject name is required.")]
        [Display(Name = "Subject Name")]
        public string SubjectName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Number of units is required.")]
        [Range(1, 6, ErrorMessage = "Units must be between 1 and 6.")]
        public int Units { get; set; }

        [Required(ErrorMessage = "Please specify the assigned instructor.")]
        [Display(Name = "Assigned Instructor")]
        public string InstructorName { get; set; } = string.Empty;
    }
}