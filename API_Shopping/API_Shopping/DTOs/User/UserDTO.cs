namespace API_Shopping.DTOs.User
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }    
        public bool? IsActive{ get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
