namespace API_Shopping.Exceptions.ShoppingCart
{
    public class InvalidQuantityException : AppException
    {
        public InvalidQuantityException()
            : base(StatusCodes.Status400BadRequest, "Invalid quantity", "Quantity must be zero or greater.") { }
    }
}