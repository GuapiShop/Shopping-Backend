namespace API_Shopping.Exceptions.User
{
    public class UserNotFoundException : AppException
    {
        public UserNotFoundException(long id)
            : base(StatusCodes.Status404NotFound, "User not found", $"No user was found with ID {id}.") { }
    }
}