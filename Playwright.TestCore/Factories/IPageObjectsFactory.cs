namespace SkbKontur.Playwright.TestCore.Factories;

/// <summary>
/// Интерфейс композитной фабрики для создания page objects.
/// Объединяет фабрики страниц и контролов.
/// </summary>
public interface IPageObjectsFactory
{
    /// <summary>
    /// Фабрика для создания страниц
    /// </summary>
    IPageFactory PageFactory { get; }

    /// <summary>
    /// Фабрика для создания контролов
    /// </summary>
    IControlFactory ControlFactory { get; }
}