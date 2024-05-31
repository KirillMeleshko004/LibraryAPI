namespace Identity.UseCases.Common.Exceptions
{
   public class SecretKeyNotSetException : Exception
   {
      public SecretKeyNotSetException() :
         base("Secret key is not set (Should be defined as enviromental LIBRARY_SECRET)")
      {
         
      }
   }
}