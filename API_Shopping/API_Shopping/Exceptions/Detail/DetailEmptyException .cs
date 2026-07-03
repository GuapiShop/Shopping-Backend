namespace API_Shopping.Exceptions.Detail
{
    public class DetailEmptyException : AppException
    {
        public DetailEmptyException()
            : base(StatusCodes.Status400BadRequest, "Empty order", "At least one product is required to place an order.") { }
    }
}