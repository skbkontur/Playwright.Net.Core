using System.Threading;
using System.Threading.Tasks;

namespace SkbKontur.Playwright.TestCore.Auth;

/// <summary>
/// Стратегия аутентификации с кэшированием состояния.
/// Выполняет аутентификацию один раз и кэширует результат для повторного использования.
/// </summary>
/// <param name="authenticator">Аутентификатор для выполнения процесса аутентификации</param>
public class AuthWithCacheStrategy(IAuthenticator authenticator) : IAuthStrategy
{
    /// <summary>
    /// Семафор для синхронизации доступа к кэшу состояния.
    /// </summary>
    private static readonly SemaphoreSlim Semaphore = new(1, 1);
    
    /// <summary>
    /// Кэшированное состояние хранения браузера.
    /// </summary>
    private static string? _cachedStorageState = null;

    /// <summary>
    /// Флаг, указывающий, была ли выполнена инициализация.
    /// </summary>
    private static bool _isInitialized = false;

    /// <summary>
    /// Получить кэшированное состояние.
    /// При первом вызове выполняет аутентификацию и кэширует результат.
    /// </summary>
    /// <returns>JSON строка с состоянием хранения или null</returns>
    public async Task<string?> GetOrCreateStorageStateAsync()
    {
        if (_isInitialized)
        {
            return _cachedStorageState;
        }
        await Semaphore.WaitAsync();
        try
        {
            if (!_isInitialized)
            {
                _cachedStorageState = await authenticator.CreateStorageStateAsync();
                _isInitialized = true;
            }
        }
        finally
        {
            Semaphore.Release();
        }
        return _cachedStorageState;
    }
}