using BlogsPlatform.Abstractions.Localization;

namespace BlogsPlatform.Services;

public class LanguageContext(IHttpContextAccessor _httpContextAccessor) : ILanguageContext
{
    private const string LanguageHeader = "X-Core-Language";

    public string CurrentLanguage
    {
        get
        {
            return _httpContextAccessor.
                HttpContext?.
                Request.
                Headers[LanguageHeader].ToString() ?? "EN";
        }
    }

    public void SetLanguage(string languageCode)
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            _httpContextAccessor.
                HttpContext.
                Response.
                Headers[LanguageHeader] = languageCode;
        }
    }
}