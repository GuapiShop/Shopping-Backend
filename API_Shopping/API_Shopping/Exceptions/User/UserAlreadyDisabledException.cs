namespace API_Shopping.Exceptions.User
{
    public class UserAlreadyDisabledException : AppException
    {
        public UserAlreadyDisabledException(long id)
            : base(StatusCodes.Status409Conflict, "User already disabled", $"User with ID {id} is already inactive."){ }
    }
}