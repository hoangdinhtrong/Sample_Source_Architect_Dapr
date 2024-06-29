using MimeKit;
using MimeKit.Utils;
using SampeDapr.Domain;
using System.Text.RegularExpressions;

namespace SampeDapr.Application.Shared.Extensions
{
    public static class EmailExtension
    {
        public static string ConvertImagePathToBase64(this string emailBody)
        {
            foreach (var item in GetAllImgs(emailBody))
            {
                var last = item.LastIndexOf("/") + 1;
                var imgName = item.Substring(last, item.Length - last);
                var localImg = File.ReadAllBytes(DomainConstansts.ImgConstants.Directory + imgName);
                var base64 = Convert.ToBase64String(localImg);
                var newString = string.Format(DomainConstansts.ImgConstants.Base64ImgFormat, base64);

                emailBody = emailBody.Replace(item, newString);
            }

            return emailBody;
        }

        public static string ConvertImagePathToCid(this string emailBody, BodyBuilder bodyBuilder)
        {
            foreach (var item in GetAllImgs(emailBody))
            {
                var last = item.LastIndexOf("/") + 1;
                var imgName = item.Substring(last, item.Length - last);
                string imagePath = Path.Combine(DomainConstansts.ImgConstants.Directory, imgName);
                MimeEntity image = bodyBuilder.LinkedResources.Add(imagePath);
                image.ContentId = MimeUtils.GenerateMessageId();
                emailBody = emailBody.Replace(item, $@"cid:{image.ContentId}");
            }

            return emailBody;
        }

        public static string BuildBodyTemplate(this string emailBody, Dictionary<string, string>? replaceFields)
        {
            if (replaceFields is null || replaceFields.Count <= 0) 
                return emailBody;

            foreach (var item in replaceFields)
            {
                emailBody = emailBody.Replace(item.Key, item.Value);
            }

            return emailBody.ConvertImagePathToBase64();
        }

        public static string BuildBodyTemplate(this string emailBody,
            Dictionary<string, string>? replaceFields,
            BodyBuilder bodyBuilder)
        {
            if (replaceFields is null || replaceFields.Count <= 0) 
                return emailBody;

            foreach (var item in replaceFields)
            {
                emailBody = emailBody.Replace(item.Key, item.Value);
            }
            return emailBody.ConvertImagePathToCid(bodyBuilder);
        }

        public static bool ValidateEmail(this string? emailInput)
        {
            if (string.IsNullOrWhiteSpace(emailInput)) 
                return false;

            emailInput = emailInput.Trim();
            string REGULAR_EMAIL = "^[a-z0-9][-a-z0-9._]+@([-a-z0-9]+\\.)+[a-z]{2,5}$";
            return Regex.IsMatch(emailInput, REGULAR_EMAIL, RegexOptions.IgnoreCase);
        }

        private static IEnumerable<string> GetAllImgs(this string emailBody)
        {
            string regexImgSrc = string.Format(@"<\s*img\s*src\s*=\s*{0}\s*([^{0}]+)\s*{0}", "\"");
            MatchCollection matchesImgSrc = Regex.Matches(emailBody, regexImgSrc,
                RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Multiline);
            foreach (Match m in matchesImgSrc)
            {
                string href = m.Groups[1].Value;
                yield return href;
            }
        }
    }
}
