namespace Library.UseCases.Exceptions
{
    public class NotFoundException(string message) : Exception(message);
}