namespace LibraryAPI.LibraryService.Shared.RequestFeatures
{
    /// <summary>
    /// Base class for all request parameters
    /// Includes base request parameters such as page info and order query
    /// </summary>
    public abstract class RequestParameters
    {
        const int MAX_PAGE_SIZE = 50;

        public int PageNumber { get; set; }

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
                _pageSize = value > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : value;
            }
        }

        public string? OrderBy { get; set; }
    }
}