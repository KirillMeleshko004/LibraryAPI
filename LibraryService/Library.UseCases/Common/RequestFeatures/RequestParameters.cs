using System.ComponentModel.DataAnnotations;

namespace Library.UseCases.Common.RequestFeatures
{
    /// <summary>
    /// Base class for all request parameters
    /// </summary>
    public abstract class RequestParameters
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? OrderBy { get; set; }
        public string? SearchTerm { get; set; }
    }
}