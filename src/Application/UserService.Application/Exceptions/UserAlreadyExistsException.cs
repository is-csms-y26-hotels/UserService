namespace UserService.Application.Exceptions;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException() : base("User with the same email already exists.")
    {
    }

    public UserAlreadyExistsException(string message) : base(message)
    {
    }

    public UserAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}