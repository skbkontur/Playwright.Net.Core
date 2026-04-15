using System.Threading.Tasks;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore;

/// <summary>
/// Интерфейс фабрики для создания и управления экземплярами Playwright.
/// Обеспечивает ленивую инициализацию и кэширование экземпляра Playwright.
/// </summary>
public interface IPlaywrightGetter
{
    /// <summary>
    /// Асинхронно получить экземпляр Playwright.
    /// При первом вызове создаёт новый экземпляр, последующие вызовы возвращают кэшированный.
    /// </summary>
    /// <returns>Задача, возвращающая экземпляр IPlaywright</returns>
    Task<IPlaywright> GetPlaywrightAsync();
}