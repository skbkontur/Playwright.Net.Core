using System.Threading.Tasks;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Browsers;

/// <summary>
/// Интерфейс для получения экземпляра браузера.
/// Определяет контракт для создания и управления браузерами.
/// </summary>
public interface IBrowserGetter
{
    /// <summary>
    /// Получить экземпляр браузера.
    /// При первом вызове создаёт новый экземпляр, последующие вызовы возвращают кэшированный.
    /// </summary>
    /// <returns>Задача, возвращающая экземпляр браузера</returns>
    Task<IBrowser> GetAsync();
}