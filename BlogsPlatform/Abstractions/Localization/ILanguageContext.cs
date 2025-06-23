namespace BlogsPlatform.Abstractions.Localization;

public interface ILanguageContext
{
    string CurrentLanguage { get; }
    void SetLanguage(string languageCode);
}