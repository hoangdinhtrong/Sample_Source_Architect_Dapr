using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using SampeDapr.Application.Shared.Dtos;
using SampeDapr.Application.Shared.Extensions;
using SampeDapr.Application.Shared.Interfaces;
using SampeDapr.Domain;
using System.Net;

namespace SampeDapr.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _accessKey;
        private readonly string _region;
        private readonly ILogger<EmailService> _logger;
        private readonly string _secretKey;
        private readonly string _emailFrom;
        public EmailService(IConfiguration configuration,
            ILogger<EmailService> logger)
        {
            _accessKey = configuration["AWS:SES:AccessKey"];
            _secretKey = configuration["AWS:SES:SecretKey"];
            _region = configuration["AWS:SES:Region"];
            _emailFrom = configuration["EmailSettings:DefaultFrom"];
            _logger = logger;
        }
        public async Task<bool> SendEmailAsync(SendEmailDto request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.SenderEmail))
                {
                    request.SenderEmail = _emailFrom;
                }
                if (request.ToAddresses is null || !request.ToAddresses.Any()
                    || !request.SenderEmail.ValidateEmail())
                    return false;

                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress(request.SenderEmail, request.SenderEmail));
                foreach (var toAddresss in request.ToAddresses)
                {
                    if (!toAddresss.ValidateEmail()) continue;
                    message.To.Add(new MailboxAddress(toAddresss, toAddresss));
                }
                if (request.CcAddresses?.Count > 0)
                {
                    foreach (var ccAddress in request.CcAddresses)
                    {
                        if (!ccAddress.ValidateEmail()) continue;
                        message.Cc.Add(new MailboxAddress(ccAddress, ccAddress));
                    }
                }
                BodyBuilder bodyBuilder = new();
                message.Subject = request.Subject;
                bodyBuilder.HtmlBody = request.ContentEmail.ContentTemplate.BuildBodyTemplate(request.ContentEmail.ReplaceFields, bodyBuilder);
                message.Body = bodyBuilder.ToMessageBody();
                return await SendRawEmailAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> SendEmailAsync(string sendTo, EmailTemplate emailTemplate, Dictionary<string, string>? bodyMap)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sendTo)) return false;
                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailFrom, _emailFrom));
                message.To.Add(new MailboxAddress(sendTo, sendTo));
                BodyBuilder bodyBuilder = new();
                message.Subject = emailTemplate.Subject;
                bodyBuilder.HtmlBody = emailTemplate.Body?.BuildBodyTemplate(bodyMap, bodyBuilder);
                message.Body = bodyBuilder.ToMessageBody();
                return await SendRawEmailAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> SendRawEmailAsync(MimeMessage message)
        {
            using (AmazonSimpleEmailServiceClient client = new(_accessKey, _secretKey, RegionEndpoint.GetBySystemName(_region)))
            {
                using (var memoryStream = new MemoryStream())
                {
                    message.WriteTo(memoryStream);
                    var sendRequest = new SendRawEmailRequest { RawMessage = new RawMessage(memoryStream) };

                    var response = await client.SendRawEmailAsync(sendRequest);

                    if (response.HttpStatusCode == HttpStatusCode.OK) return true;

                    _logger.LogError($"Failed to send email {message.Subject} : {message.To}.");
                    throw new Exception($"Failed to send email {message.Subject} : {message.To}.");
                }
            }
        }
    }
}
