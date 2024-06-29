namespace SampeDapr.Application.Shared.Dtos
{
    public class BaseResponse<T> where T : class
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public bool Success { get; set; } = true;
        public List<string>? Error { get; set; }
        public T? Data { get; set; }
    }
}
