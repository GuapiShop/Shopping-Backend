using Microsoft.AspNetCore.Http;

namespace API_Shopping.Exceptions.Product
{
    public class ProductNotFoundException : AppException
    {
        public ProductNotFoundException(string message)
            : base(StatusCodes.Status404NotFound, "Product not found", message) { }
    }
}