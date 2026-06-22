using Microsoft.AspNetCore.Http;

namespace API_Shopping.Exceptions.Product
{
    public class ProductNotFoundException : AppException
    {
        public ProductNotFoundException(long id)
            : base(StatusCodes.Status404NotFound, "Product not found", $"No product was found.") { }
    }
}