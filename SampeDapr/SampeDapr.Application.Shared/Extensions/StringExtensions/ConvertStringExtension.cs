using System.Globalization;

namespace SampeDapr.Application.Shared.Extensions
{
    public static class ConvertStringExtension
    {
        public static DateTime? ConvertStringToDate(string? fieldValue)
        {
            if(string.IsNullOrWhiteSpace(fieldValue)) return null;

            if (DateTime.TryParse(fieldValue, out DateTime dateValue))
                return dateValue;

            if (DateTime.TryParseExact(fieldValue,
                    new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "d/M/yyyy", "M/yyyy" },
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                return dateValue;

            if (DateTime.TryParse(fieldValue.Trim(), out dateValue))
                return dateValue;

            return null;
        }

        public static string? FormatDate(DateTime? value, string formatOption)
        {
            return value.HasValue ? value.Value.ToString(formatOption) : null;
        }

        public static string? ConvertUTCDateTime(this DateTime? value)
        {
            if (!value.HasValue) 
                return null;

            return value.Value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        }

        public static float? ConvertStringToFloat(this string? obj)
        {
            if (string.IsNullOrWhiteSpace(obj)
                || !float.TryParse(obj, out float value)) 
                return null;

            return value;
        }

        public static string? ConvertToFirstUpperCase(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) 
                return null;

            string uppper = value.Substring(0, 1).ToUpper();
            string lastCharacter = value.Substring(1, value.Length - 1);
            return $"{uppper}{lastCharacter}";
        }

        public static decimal? ConvertStringToDecimal(this string? value)
        {
            if (string.IsNullOrWhiteSpace(value)
                || !decimal.TryParse(value.Trim(), out decimal result)) 
                return null;

            return result;
        }

        public static DateTime? ConvertStringToDateTime(this string? fieldValue)
        {
            if (string.IsNullOrWhiteSpace(fieldValue)) return null;

            if (DateTime.TryParse(fieldValue.Trim(), out DateTime dateValue))
                return dateValue;

            if (DateTime.TryParseExact(fieldValue,
                    new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "d/M/yyyy", "M/yyyy" },
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                return dateValue;

            return null;
        }
    }
}
