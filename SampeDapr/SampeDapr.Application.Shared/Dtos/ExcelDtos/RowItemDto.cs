namespace SampeDapr.Application.Shared.Dtos
{
    public class RowItemDto<T>
    {
        public int RowNumber { set; get; }
        public T? Item { set; get; }
    }
}
