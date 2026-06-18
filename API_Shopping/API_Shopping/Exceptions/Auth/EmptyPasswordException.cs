namespace API_Shopping.Exceptions.Auth
{
    public class EmptyPasswordException : AppException
    {
        public EmptyPasswordException()
            : base(StatusCodes.Status400BadRequest, "Password required", "Password field is required.") { }
    }
}