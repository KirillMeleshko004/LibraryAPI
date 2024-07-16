namespace Library.UseCases.Exceptions
{
    public class ForbidException(string message) : Exception(message);
}