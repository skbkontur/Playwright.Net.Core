using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Auth;

/// <summary>
/// Интерфейс стратегии аутентификации для браузерных контекстов.
/// Определяет методы для получения параметров контекста и состояния хранения.
/// </summary>
public interface IAuthStrategy
{
    /// <summary>
    /// Получить или создать параметры для нового контекста браузера.
    /// Может включать сохранённое состояние аутентификации.
    /// </summary>
    /// <returns>Параметры для создания нового контекста браузера</returns>
    BrowserNewContextOptions GetOrCreateContextOptionsAsync();

    /// <summary>
    /// Получить или создать состояние хранения браузера.
    /// Возвращает JSON строку с сохранёнными cookies, localStorage и sessionStorage.
    /// </summary>
    /// <returns>JSON строка с состоянием хранения или null</returns>
    string? GetOrCreateStorageStateAsync();
}