namespace API_Shopping.Exceptions.Detail
{
    public class DetailOrderNotFoundException : AppException
    {
        public DetailOrderNotFoundException(long userId)
            : base(StatusCodes.Status404NotFound, "No pending orders", $"No pending orders were found for user with ID {userId}.") { }
    }
}