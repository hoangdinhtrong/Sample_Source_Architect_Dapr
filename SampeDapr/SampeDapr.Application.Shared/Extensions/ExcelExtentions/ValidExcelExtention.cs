using SampeDapr.Application.Shared.Dtos;
using System.Net.Sockets;

namespace SampeDapr.Application.Shared.Extensions
{
    public static class ValidExcelExtention
    {
        public static bool IsValidURL(string url)
        {
            return IsUrlDownloadFile(url);
        }

        public static bool IsUrlDownloadFile(string url)
        {
            using var client = new HttpClient();
            try
            {
                var response = client.Send(new HttpRequestMessage(HttpMethod.Head, url));
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
                when (e.InnerException is SocketException
                { SocketErrorCode: SocketError.HostNotFound })
            {
                return false;
            }
            catch (HttpRequestException e)
                when (e.StatusCode.HasValue && (int)e.StatusCode.Value > 500)
            {
                return true;
            }
        }

        public static bool IsFieldInList(string? fieldValue, IEnumerable<string>? groupValue)
        {
            if (string.IsNullOrEmpty(fieldValue?.Trim())) return false;

            if (groupValue is null || !groupValue.Any()) return false;

            if (groupValue.Count(x => fieldValue.Trim().Equals(x, StringComparison.OrdinalIgnoreCase)) != 1) return false;

            return true;
        }

        public static void ValidateField(int cellIndex, 
            string errorCode, 
            string errorMessage,
            ImportDataValidateResultDto finalResult)
        {
            finalResult.IsValid = false;
            finalResult.Errors?.Add(new ValidateErrorDto()
            {
                CellIndex = cellIndex,
                ErrorCode = errorCode,
                ErrorMessage = errorMessage
            });
        }
    }
}
