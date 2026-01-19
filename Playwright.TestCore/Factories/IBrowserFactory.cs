using System.Threading.Tasks;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Factories;

/// <summary>
/// Интерфейс фабрики для создания контекстов браузеров.
/// Определяет контракт для различных реализаций создания браузерных контекстов.
/// </summary>
public interface IBrowserFactory
{
    /// <summary>
    /// Асинхронно создать новый контекст браузера.
    /// </summary>
    /// <returns>Задача, возвращающая созданный IBrowserContext</returns>
    Task<IBrowserContext> CreateAsync();
}