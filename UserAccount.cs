using System.ComponentModel.DataAnnotations;

namespace InstructorInfoSystem.Models
{
    public class UserAccount
    {
        public int Id { get; set; }

        [Required]
        public string InstructorId { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string NormalizedEmail { get; set; } = string.Empty;

        [Required]
        public string NormalizedInstructorId { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
