using BlogsPlatform.Abstractions;
using BlogsPlatform.Models;

namespace BlogsPlatform.Services;

public class EmailService : IEmailService
{
    public Task<Result<SendEmailResponse>> SendEmail(SendEmailRequest request)
    {
        throw new NotImplementedException();
    }
}