using SampeDapr.Application.Shared.Interfaces;

namespace SampeDapr.Application.Shared.Dtos
{
    [Serializable]
    public class PagedResultDto<T> : ListResultDto<T>, IPageResult<T>
    {
        /// <summary>
        /// inheric doc
        /// </summary>
        public long TotalCount { get; set; }
        /// <summary>
        /// Create a new object <see cref="PagedResultDto{T}" ></see>
        /// </summary>
        public PagedResultDto()
        {

        }
        /// <summary>
        ///create a new <see cref="PagedResultDto{T}"/> object
        /// </summary>
        /// <param name="totalCount">Total count Of item</param>
        /// <param name="items">Items in page</param>
        public PagedResultDto(long totalCount, IReadOnlyList<T> items) : base(items)
        {
            this.TotalCount = totalCount;
        }
    }
}
