namespace SkbKontur.Playwright.POM.Abstractions;

public interface IPageWrapper<out TPage> : IWrapper<TPage>
{
    string Url { get; }
}