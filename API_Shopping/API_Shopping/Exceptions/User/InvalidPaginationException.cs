using Microsoft.AspNetCore.Http;

namespace API_Shopping.Exceptions.User
{
    public class InvalidPaginationException : AppException
    {
        public InvalidPaginationException()
            : base(StatusCodes.Status400BadRequest, "Invalid pagination parameters", "Page and pageSize must be greater than 0.") { }
    }
}