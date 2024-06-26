using System.ComponentModel.DataAnnotations;

namespace Library.UseCases.Common.RequestFeatures
{
    /// <summary>
    /// Base class for all request parameters
    /// </summary>
    public abstract class RequestParameters
    {
        const int MAX_PAGE_SIZE = 50;
        const int MIN_PAGE_SIZE = 1;

        [Range(1, Int32.MaxValue, ErrorMessage = "Page number must be not negative")]
        public int PageNumber { get; set; } = 1;


        //Default page size
        private int _pageSize = 10;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                if (value > MAX_PAGE_SIZE) _pageSize = MAX_PAGE_SIZE;
                else if (value < MIN_PAGE_SIZE) _pageSize = MIN_PAGE_SIZE;
                else _pageSize = value;
            }
        }

        [MaxLength(100, ErrorMessage = "Order string max length is 100")]
        public string? OrderBy { get; set; }

        [MaxLength(50, ErrorMessage = "Search term max length is 50")]
        public string? SearchTerm { get; set; }
    }
}