using System.ComponentModel.DataAnnotations;

namespace API_Shopping.DTOs.User
{
    public class UserUpdateDTO
    {
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "ID must be a positive number.")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email format is not valid.")]
        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
        public required  string Email { get; set; }

        public DateTime? UpdateAt { get; set; }
    }
}