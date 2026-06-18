using Microsoft.AspNetCore.Http;

namespace API_Shopping.Exceptions.Auth
{
    public class InvalidCredentialsException : AppException
    {
        public InvalidCredentialsException()
            : base(StatusCodes.Status404NotFound, "Bad credentials", "No user found matching the provided email and password.") { }
    }
}
