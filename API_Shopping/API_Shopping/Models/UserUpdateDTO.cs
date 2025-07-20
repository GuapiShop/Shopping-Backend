namespace API_Shopping.Models
{
    public class UserUpdateDTO
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
