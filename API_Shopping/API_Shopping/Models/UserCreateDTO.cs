namespace API_Shopping.Models
{
    public class UserCreateDTO
    {
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public DateTime? createAt { get; set; }
        public DateTime? updateAt { get; set; }
    }
}
