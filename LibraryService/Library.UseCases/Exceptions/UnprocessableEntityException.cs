namespace Library.UseCases.Exceptions
{
    public class UnprocessableEntityException(string message) : Exception(message);
}