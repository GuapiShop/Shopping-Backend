namespace API_Shopping.Exceptions.Product
{
    public class ProductUpdateException : AppException
    {
        public ProductUpdateException(long id)
            : base(StatusCodes.Status400BadRequest, "Product update failed", $"The product could not be updated.") { }
    }
}
