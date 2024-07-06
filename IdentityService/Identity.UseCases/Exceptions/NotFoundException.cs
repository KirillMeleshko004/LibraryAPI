namespace Identity.UseCases.Exceptions
{
    public class NotFoundException(string message) : Exception(message);
}