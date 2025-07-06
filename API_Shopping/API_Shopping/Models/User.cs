namespace API_Shopping.Models
{
    public class User
    {
        public long ?Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool? IsActive{ get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
