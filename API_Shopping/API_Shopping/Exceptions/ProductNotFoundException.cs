namespace API_Shopping.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(string message) : base(message){ }
    }
}
