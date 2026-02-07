using System.Threading.Tasks;

namespace SkbKontur.Playwright.TestCore.Auth;

/// <summary>
/// Стратегия без аутентификации.
/// Используется для сценариев, где аутентификация не требуется.
/// </summary>
public class WithoutAuthStrategy : IAuthStrategy
{
    /// <summary>
    /// Получить состояние аутентификации (всегда возвращает null).
    /// </summary>
    /// <returns>Null, так как аутентификация не выполняется</returns>
    public Task<string?> GetOrCreateStorageStateAsync()
        => null;
}