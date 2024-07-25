using Library.UseCases.Common.Interfaces;

namespace Library.UseCases.Common.Utility
{
    public class PagedList<T> : List<T>, IPagedEnumerable<T>
    {
        public IEnumerable<T> Items { get; }
        public PageMetaData Pages { get; }

        public PagedList(IEnumerable<T> items, int pageSize, int pageNumber, int totalCount)
        {
            Pages = new PageMetaData()
            {
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            Items = items;

            AddRange(items);
        }

        public PagedList(IEnumerable<T> items, PageMetaData metaData)
        {
            Pages = metaData;

            Items = items;

            AddRange(items);
        }
    }
}