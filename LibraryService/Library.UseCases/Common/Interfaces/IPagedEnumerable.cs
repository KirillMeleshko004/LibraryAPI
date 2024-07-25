using Library.UseCases.Common.Utility;

namespace Library.UseCases.Common.Interfaces
{
    public interface IPagedEnumerable<T> : IEnumerable<T>
    {
        public IEnumerable<T> Items { get; }
        public PageMetaData Pages { get; }
    }
}