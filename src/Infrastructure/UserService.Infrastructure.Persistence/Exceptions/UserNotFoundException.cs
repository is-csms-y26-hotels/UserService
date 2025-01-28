namespace UserService.Application.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException() : base("User with the same email already exists.")
    {
    }

    public UserNotFoundException(string message) : base(message)
    {
    }

    public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}