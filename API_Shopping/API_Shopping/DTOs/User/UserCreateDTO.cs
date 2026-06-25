using System.ComponentModel.DataAnnotations;

namespace API_Shopping.DTOs.User
{
    public class UserCreateDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email format is not valid.")]
        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
        public required string Email { get; set; }
    }
}