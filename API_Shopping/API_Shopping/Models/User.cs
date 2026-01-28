using Microsoft.AspNetCore.Identity;

namespace API_Shopping.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? IsActive{ get; set; }
        public string Role { get; set; } 
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
