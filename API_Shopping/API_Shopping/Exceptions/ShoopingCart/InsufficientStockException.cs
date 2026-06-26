namespace API_Shopping.Exceptions.ShoppingCart
{
    public class InsufficientStockException : AppException
    {
        public InsufficientStockException(long productId, int available)
            : base(StatusCodes.Status422UnprocessableEntity, "Insufficient stock", $"Product with ID {productId} only has {available} units available.") { }
    }
}