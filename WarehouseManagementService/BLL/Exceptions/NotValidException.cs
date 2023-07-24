namespace BLL.Exceptions
{
    public class NotValidException : Exception
    {
        public NotValidException()
        {
        }

        public NotValidException(string message)
            : base(message)
        {
        }

        public NotValidException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
