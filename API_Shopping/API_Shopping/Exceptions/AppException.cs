namespace API_Shopping.Exceptions
{
    public abstract class AppException : Exception
    {
        public int StatusCode { get; }
        public string Title { get; }

        protected AppException(int statusCode, string title, string detail) : base(detail)
        {
            StatusCode = statusCode;
            Title = title;
        }
    }
}