using System.Threading.Tasks;

namespace SkbKontur.Playwright.TestCore.Auth;

/// <summary>
/// Аутентификатор без выполнения аутентификации.
/// Используется для сценариев, где аутентификация не требуется.
/// </summary>
public class WithoutAuthAutentificator : IAutentificator
{
    /// <summary>
    /// Не выполняет никакой аутентификации.
    /// </summary>
    /// <returns>Задача, возвращающая null</returns>
    public Task<string?> CreateStorageStateAsync()
        => Task.FromResult<string?>(null);
}