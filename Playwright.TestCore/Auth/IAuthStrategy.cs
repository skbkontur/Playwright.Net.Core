using System.Threading.Tasks;

namespace SkbKontur.Playwright.TestCore.Auth;

/// <summary>
/// Интерфейс стратегии аутентификации для браузерных контекстов.
/// Определяет методы для получения параметров контекста и состояния хранения.
/// </summary>
public interface IAuthStrategy
{
    /// <summary>
    /// Получить или создать состояние хранения браузера.
    /// Возвращает JSON строку с сохранёнными cookies, localStorage и sessionStorage.
    /// </summary>
    /// <returns>JSON строка с состоянием хранения или null</returns>
    Task<string?> GetOrCreateStorageStateAsync();
}