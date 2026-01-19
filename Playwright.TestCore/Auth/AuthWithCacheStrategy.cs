using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Auth;

/// <summary>
/// Стратегия аутентификации с кэшированием состояния.
/// Выполняет аутентификацию один раз и кэширует результат для повторного использования.
/// </summary>
/// <param name="autentificator">Аутентификатор для выполнения процесса аутентификации</param>
public class AuthWithCacheStrategy(IAutentificator autentificator) : IAuthStrategy
{
    /// <summary>
    /// Объект для синхронизации доступа к кэшу состояния.
    /// </summary>
    private static readonly object Lock = new();

    /// <summary>
    /// Кэшированное состояние хранения браузера.
    /// </summary>
    private static string? _cachedStorageState = null;

    /// <summary>
    /// Флаг, указывающий, была ли выполнена инициализация.
    /// </summary>
    private static bool isInitialized = false;

    /// <summary>
    /// Получить параметры контекста браузера с кэшированным состоянием аутентификации.
    /// </summary>
    /// <returns>Параметры контекста с сохранённым состоянием хранения</returns>
    public BrowserNewContextOptions GetOrCreateContextOptionsAsync()
    {
        var state = GetOrCreateStorageStateAsync();
        return new BrowserNewContextOptions { StorageState = state };
    }

    /// <summary>
    /// Получить кэшированное состояние.
    /// При первом вызове выполняет аутентификацию и кэширует результат.
    /// </summary>
    /// <returns>JSON строка с состоянием хранения или null</returns>
    public string? GetOrCreateStorageStateAsync()
    {
        if (isInitialized || _cachedStorageState != null)
        {
            return _cachedStorageState;
        }

        lock (Lock)
        {
            if (!isInitialized)
            {
                _cachedStorageState ??= autentificator.CreateStorageStateAsync().GetAwaiter().GetResult();
                isInitialized = true;
            }
        }

        return _cachedStorageState;
    }
}