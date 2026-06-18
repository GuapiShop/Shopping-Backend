using Microsoft.AspNetCore.Http;

namespace API_Shopping.Exceptions.Auth
{
    public class EmptyEmailException : AppException
    {
        public EmptyEmailException()
            : base(StatusCodes.Status400BadRequest, "Email required", "Email field is required.") { }
    }
}
