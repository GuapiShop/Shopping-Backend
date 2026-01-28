namespace API_Shopping.DTOs.User
{
    public class UserUpdateDTO
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
