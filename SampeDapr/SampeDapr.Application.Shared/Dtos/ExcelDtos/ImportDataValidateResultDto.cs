namespace SampeDapr.Application.Shared.Dtos
{
    public class ImportDataValidateResultDto
    {
        public bool IsValid { get; set; }
        public List<ValidateErrorDto>? Errors { get; set; }
    }
}
