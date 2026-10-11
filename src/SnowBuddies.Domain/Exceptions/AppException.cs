namespace SnowBuddies.Domain.Exceptions
{
    /// <summary>
    /// An expected error whose message is safe to return to the client with the given HTTP status code.
    /// </summary>
    public abstract class AppException : Exception
    {
        protected AppException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; }
    }

    public class BadRequestException : AppException
    {
        public BadRequestException(string message) : base(message, 400) { }
    }

    public class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message) : base(message, 401) { }
    }

    public class ForbiddenException : AppException
    {
        public ForbiddenException(string message) : base(message, 403) { }
    }

    public class NotFoundException : AppException
    {
        public NotFoundException(string message) : base(message, 404) { }
    }

    public class ConflictException : AppException
    {
        public ConflictException(string message) : base(message, 409) { }
    }
}
