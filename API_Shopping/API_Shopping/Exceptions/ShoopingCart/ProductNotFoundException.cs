namespace API_Shopping.Exceptions.ShoppingCart
{
    public class ProductNotFoundException : AppException
    {
        public ProductNotFoundException(long productId)
            : base(StatusCodes.Status404NotFound, "Product not found", $"Product with ID {productId} was not found.") { }
    }
}