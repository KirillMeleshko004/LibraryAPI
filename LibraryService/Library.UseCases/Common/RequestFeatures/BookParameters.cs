namespace Library.UseCases.Common.RequestFeatures
{
    public class BookParameters : RequestParameters
    {
        public string[]? Authors { get; set; }
        public string[]? Genres { get; set; }

    }
}