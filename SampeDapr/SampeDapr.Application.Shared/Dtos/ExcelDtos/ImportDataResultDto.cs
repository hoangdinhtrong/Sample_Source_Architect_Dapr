namespace SampeDapr.Application.Shared.Dtos
{
    public class ImportDataResultDto
    {
        public string? ErrorExcelFile { get; set; }
        public bool IsInvalidTemplate { get; set; }
        public List<ImportSheetDataResultDto>? ImportSheetDataResults { get; set; }
    }
}
