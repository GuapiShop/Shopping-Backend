namespace API_Shopping.Exceptions.ShoppingCart
{
    public class InvalidUserIdException : AppException
    {
        public InvalidUserIdException()
            : base(StatusCodes.Status400BadRequest, "Invalid user ID", "User ID must be a positive number.") { }
    }
}