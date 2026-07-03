namespace API_Shopping.Exceptions.ShoppingCart
{
    public class EmptyCartException : AppException
    {
        public EmptyCartException()
            : base(StatusCodes.Status400BadRequest, "Empty cart",
                "Your shopping cart has no items to process.")
        { }
    }
}