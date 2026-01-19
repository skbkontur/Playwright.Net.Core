using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Configurations;

/// <summary>
/// Интерфейс конфигуратора параметров запуска браузера.
/// Определяет параметры для обычного и персистентного запуска браузера.
/// </summary>
public interface IBrowserConfigurator
{
    /// <summary>
    /// Получить параметры для обычного запуска браузера.
    /// </summary>
    /// <returns>Параметры запуска браузера</returns>
    BrowserTypeLaunchOptions GetLaunchOptions();

    /// <summary>
    /// Получить параметры для запуска браузера с персистентным контекстом.
    /// </summary>
    /// <returns>Параметры запуска браузера с персистентным контекстом</returns>
    BrowserTypeLaunchPersistentContextOptions GetLaunchPersistentContextOptions();
}