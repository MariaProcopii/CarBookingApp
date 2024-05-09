namespace CarBookingApp.Application.Common.Exceptions;

public class EntityNotValidException : Exception
{
    public EntityNotValidException(string message) : base(message)
    {
    }
}