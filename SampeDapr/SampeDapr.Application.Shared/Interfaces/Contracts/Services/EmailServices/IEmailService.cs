using MimeKit;
using SampeDapr.Application.Shared.Dtos;
using SampeDapr.Domain;

namespace SampeDapr.Application.Shared.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendRawEmailAsync(MimeMessage message);
        Task<bool> SendEmailAsync(SendEmailDto request);
        Task<bool> SendEmailAsync(string sendTo, EmailTemplate emailTemplate, Dictionary<string, string>? bodyMap);
    }
}
