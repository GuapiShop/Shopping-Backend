namespace API_Shopping.Exceptions.User
{
    public class UserIdMismatchException : AppException
    {
        public UserIdMismatchException()
            : base(StatusCodes.Status400BadRequest, "ID mismatch", "The ID in the URL does not match the ID in the request body."){ }
    }
}