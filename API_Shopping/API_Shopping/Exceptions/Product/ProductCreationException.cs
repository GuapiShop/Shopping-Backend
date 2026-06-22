namespace API_Shopping.Exceptions.Product
{
    public class ProductCreationException : AppException
    {
        public ProductCreationException()
            : base(StatusCodes.Status400BadRequest, "Product creation failed", "The product data provided is invalid or missing.") { }
    }
}
