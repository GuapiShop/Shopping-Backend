namespace API_Shopping.Exceptions.User
{
    public class UserAlreadyExistsException : AppException
    {
        public UserAlreadyExistsException(string field, string value)
            : base(StatusCodes.Status409Conflict, "User already exists", $"A user with {field} '{value}' is already registered.") { }
    }
}