using Microsoft.AspNetCore.Http;

namespace API_Shopping.Exceptions.Product
{
    public class OutOfStockException : AppException
    {
        public OutOfStockException(string message)
            : base(StatusCodes.Status409Conflict, "Out of stock", message) { }
    }
}