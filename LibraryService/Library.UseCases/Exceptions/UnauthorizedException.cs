namespace Library.UseCases.Exceptions
{
    public class UnauthorizedException(string message) : Exception(message);
}