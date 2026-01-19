namespace SkbKontur.Playwright.POM.Abstractions;

/// <summary>
/// Интерфейс для обёрток страниц Playwright.
/// Наследуется от базового интерфейса IWrapper.
/// </summary>
/// <typeparam name="TPage">Тип страницы Playwright (IPage)</typeparam>
public interface IPageWrapper<out TPage> : IWrapper<TPage>
{
    /// <summary>
    /// URL страницы
    /// </summary>
    string Url { get; }
}