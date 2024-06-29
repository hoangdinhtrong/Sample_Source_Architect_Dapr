namespace SampeDapr.Application.Shared.Dtos
{
    public class ImportSheetDataResultDto
    {
        public string? Sheet { get; set; }
        public List<string>? ColumnHeaders { get; set; }
        public int TotalRecord { get; set; }
        public int AddCount { get; set; }
        public int UpdateCount { get; set; }
        public int ErrorCount { get; set; }
        public List<object>? Items { get; set; }
        public List<ImportErrorDto>? ImportErrors { get; set; }
        public List<RowItemDto<object>>? ErrorRowItems { set; get; }
    }
}
