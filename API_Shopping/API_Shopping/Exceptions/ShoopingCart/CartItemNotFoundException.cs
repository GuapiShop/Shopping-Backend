namespace API_Shopping.Exceptions.ShoppingCart
{
    public class CartItemNotFoundException : AppException
    {
        public CartItemNotFoundException(long itemId)
            : base(StatusCodes.Status404NotFound, "Cart item not found", $"Cart item with ID {itemId} was not found.") { }
    }
}