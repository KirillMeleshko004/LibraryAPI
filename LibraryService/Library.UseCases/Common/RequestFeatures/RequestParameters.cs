using System.ComponentModel.DataAnnotations;

namespace Library.UseCases.Common.RequestFeatures
{
    public class RequestParameters
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? OrderBy { get; set; }
    }
}