using BlogsPlatform.Abstractions;
using BlogsPlatform.Models;

namespace BlogsPlatform.Services;

public interface IEmailService
{
    Task<Result<SendEmailResponse>> SendEmail(SendEmailRequest request);
}