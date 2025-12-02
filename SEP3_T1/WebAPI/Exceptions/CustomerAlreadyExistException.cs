namespace WebAPI.Exceptions;

public class CustomerAlreadyExistException : Exception
{
    public CustomerAlreadyExistException(bool phoneExists, bool emailExists)
        : base("Customer with given phone or email already exists.")
    {
        this.phoneExists = phoneExists;
        this.emailExists = emailExists;
    }

    public bool phoneExists { get; }
    public bool emailExists { get; }
}