namespace API_Shopping.Exceptions.User
{
    public class UserAlreadyEnabledException : AppException
    {
        public UserAlreadyEnabledException(long id)
            : base(StatusCodes.Status409Conflict, "User already enabled", $"User with ID {id} is already active.") { }
    }
}