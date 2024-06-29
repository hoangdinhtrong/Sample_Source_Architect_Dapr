using System.ComponentModel.DataAnnotations;

namespace SampeDapr.Application.Shared.Dtos
{
    public class SendEmailDto
    {
        [Required]
        public string SenderEmail { get; set; } = null!;

        [Required]
        public List<string?> ToAddresses { get; set; } = null!;

        public List<string?>? CcAddresses { get; set; }

        [Required]
        public string Subject { get; set; } = null!;

        [Required]
        public ContentEmailDto ContentEmail { get; set; } = null!;
    }
    public class ContentEmailDto
    {
        [Required]
        public string ContentTemplate { get; set; } = null!;

        public Dictionary<string, string>? ReplaceFields { get; set; }
    }
}
