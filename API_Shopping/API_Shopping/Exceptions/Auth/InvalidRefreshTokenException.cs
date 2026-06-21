namespace API_Shopping.Exceptions.Auth
{
    public class InvalidRefreshTokenException : AppException
    {
        public InvalidRefreshTokenException()
            : base(StatusCodes.Status401Unauthorized, "Invalid refresh token", "The refresh token is invalid, expired, or has been revoked.") { }
    }
}