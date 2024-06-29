namespace SampeDapr.Application.Shared.Dtos
{
    public class ImportErrorDto
    {
        public int RowNumber { get; set; }
        public List<ValidateErrorDto>? Errors { get; set; }
    }

    public class ImportErrorDto<T> : ImportErrorDto where T : class
    {
        public T? Item { set; get; }
    }
}
