namespace API_Shopping.Models
{
    public class AuthResponseDTO
    {
        public string AccessToken { get; set; }
        public UserDTO User { get; set; }
    }
}
