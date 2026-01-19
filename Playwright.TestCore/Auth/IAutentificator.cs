using System.Threading.Tasks;

namespace SkbKontur.Playwright.TestCore.Auth;

/// <summary>
/// Интерфейс аутентификатора.
/// Отвечает за процесс аутентификации и сохранение состояния.
/// </summary>
public interface IAutentificator
{
    /// <summary>
    /// Создать состояние браузера после выполнения аутентификации.
    /// </summary>
    /// <returns>Задача, возвращающая JSON строку с состоянием или null</returns>
    Task<string?> CreateStorageStateAsync();
}