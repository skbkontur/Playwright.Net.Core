using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Auth;

/// <summary>
/// Стратегия без аутентификации.
/// Используется для сценариев, где аутентификация не требуется.
/// </summary>
public class WithoutAuthStrategy : IAuthStrategy
{
    /// <summary>
    /// Получить параметры контекста браузера без состояния аутентификации.
    /// </summary>
    /// <returns>Пустые параметры контекста браузера</returns>
    public BrowserNewContextOptions GetOrCreateContextOptionsAsync()
        => new BrowserNewContextOptions();

    /// <summary>
    /// Получить состояние аутентификации (всегда возвращает null).
    /// </summary>
    /// <returns>Null, так как аутентификация не выполняется</returns>
    public string? GetOrCreateStorageStateAsync()
        => null;
}