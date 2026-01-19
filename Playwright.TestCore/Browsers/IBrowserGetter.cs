using System.Threading.Tasks;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Browsers;

/// <summary>
/// Интерфейс для получения контекста браузера.
/// Определяет контракт для создания и управления браузерными контекстами.
/// </summary>
public interface IBrowserGetter
{
    /// <summary>
    /// Получить контекст браузера.
    /// При первом вызове создаёт новый контекст, последующие вызовы возвращают кэшированный.
    /// </summary>
    /// <returns>Задача, возвращающая контекст браузера</returns>
    Task<IBrowserContext> GetAsync();
}