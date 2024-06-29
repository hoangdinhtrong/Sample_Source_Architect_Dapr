using SampeDapr.Application.Shared.Dtos;

namespace SampeDapr.Application.Shared.Extensions
{
    public static class ExcelExtention
    {
        public static ImportDataResultDto ResponeExcelError(string msg, string sheetName)
        {
            ValidateErrorDto validateError = new()
            {
                ErrorMessage = msg,
                ErrorCode = "internal server"
            };

            ImportErrorDto imEr = new()
            {
                Errors = new List<ValidateErrorDto> { validateError }
            };
            ImportSheetDataResultDto sheet = new();
            sheet.Sheet = sheetName;
            sheet.ImportErrors = new List<ImportErrorDto>();
            sheet.ImportErrors.Add(imEr);
            var result = new ImportDataResultDto()
            {
                IsInvalidTemplate = true,

                ErrorExcelFile = "",
                ImportSheetDataResults = new List<ImportSheetDataResultDto> { sheet }
            };
            return result;
        }
        public static ImportErrorDto GetValidField(int rowIndex, int cellIndex, string errorCode, string errorMessage)
        {
            return new ImportErrorDto()
            {
                RowNumber = rowIndex,
                Errors = new List<ValidateErrorDto>()
                {
                    new ValidateErrorDto()
                    {
                        CellIndex = cellIndex,
                        ErrorCode = errorCode,
                        ErrorMessage = errorMessage
                    }
                }
            };
        }
    }
}
