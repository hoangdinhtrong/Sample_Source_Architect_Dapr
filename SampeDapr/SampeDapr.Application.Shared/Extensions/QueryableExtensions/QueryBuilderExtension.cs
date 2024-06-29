using System.Text;

namespace SampeDapr.Application.Shared.Extensions
{
    public static class QueryBuilderExtension
    {
        private static List<string> _vniCharacters = new List<string>() {
                "á", "à", "ả", "ã", "ạ",
                "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                "â", "ấ", "ầ", "ẩ", "ẫ", "ậ",
                "đ",
                "é", "è", "ẻ", "ẽ", "ẹ",
                "ê", "ế", "ề", "ể", "ễ", "ệ",
                "í", "ì", "ỉ", "ĩ", "ị",
                "ó", "ò", "ỏ", "õ", "ọ",
                "ô", "ố", "ồ", "ổ", "ỗ", "ộ",
                "ơ", "ớ", "ờ", "ở", "ỡ", "ợ",
                "ú", "ù", "ủ", "ũ", "ụ",
                "ư", "ứ", "ừ", "ử", "ữ", "ự",
                "ý", "ỳ", "ỷ", "ỹ", "ỵ"
        };

        public static string? GetVNIWords(this string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            var trimValue = value.ToLower().Trim();
            for (var i = trimValue.Length - 1; i >= 0; i--)
            {
                if (_vniCharacters.Any(c => c[0] == trimValue[i]))
                {
                    return trimValue.Substring(0, i + 1);
                }
            }
            return null;
        }

        public static bool CheckVNICharacters(this string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            var tempValue = value.Trim().ToLower();
            return _vniCharacters.Any(c => tempValue.Trim().ToLower().Contains(c));
        }

        public static List<string>? CreateVarianForTheLastVNIWord(this string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            var result = new List<string>();
            var vniVarianChars = new Dictionary<string, List<string>>()
            {
                {
                    "ă",
                    new List<string>() { "ắ", "ằ", "ẳ", "ẵ", "ặ" }
                },
                {
                    "â",
                    new List<string>() { "ấ", "ầ", "ẩ", "ẫ", "ậ" }
                },
                {
                    "ê",
                    new List<string>() { "ế", "ề", "ể", "ễ", "ệ" }
                },
                {
                    "ô",
                    new List<string>() { "ố", "ồ", "ổ", "ỗ", "ộ" }
                },
                {
                    "ơ",
                    new List<string>() { "ớ", "ờ", "ở", "ỡ", "ợ" }
                },
                {
                    "ư",
                    new List<string>() { "ứ", "ừ", "ử", "ữ", "ự" }
                },
            };
            var trimValue = value.ToLower().Trim();
            var words = trimValue.Split(' ');
            if (words.Length > 0)
            {
                var varians = new List<string>();
                var lastWord = words[words.Length - 1];
                if (!string.IsNullOrEmpty(lastWord))
                {
                    varians.Add(lastWord);
                    foreach (var key in vniVarianChars.Keys)
                    {
                        if (lastWord.Contains(key))
                        {
                            foreach (var varianChar in vniVarianChars[key])
                            {
                                varians.Add(lastWord.Replace(key, varianChar));
                            }
                        }
                    }

                    foreach (var varian in varians)
                    {
                        words[words.Length - 1] = varian;
                        result.Add(JoinWords(words));
                    }
                }
            }

            return result;
        }

        private static string JoinWords(this string[] words)
        {
            var wordBuilder = new StringBuilder();
            for (var i = 0; i < words.Length; i++)
            {
                if (i > 0)
                {
                    wordBuilder.Append(" ");
                }

                if (!string.IsNullOrEmpty(words[i]))
                {
                    wordBuilder.Append(words[i]);
                }
            }

            return wordBuilder.ToString();
        }
    }
}
