namespace SampeDapr.Application.Shared.Dtos
{
    public class ValidateErrorDto
    {
        public int CellIndex { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
