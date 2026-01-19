using System.Threading.Tasks;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Browsers;

/// <summary>
/// Интерфейс для управления трассировкой контекста браузера.
/// Определяет методы для начала и окончания записи трассировки.
/// </summary>
public interface IContextTracing
{
    /// <summary>
    /// Начать трассировку для указанного контекста браузера.
    /// </summary>
    /// <param name="context">Контекст браузера для трассировки</param>
    /// <returns>Задача завершения начала трассировки</returns>
    Task StartAsync(IBrowserContext context);

    /// <summary>
    /// Остановить трассировку для указанного контекста браузера.
    /// </summary>
    /// <param name="context">Контекст браузера, трассировку которого нужно остановить</param>
    /// <returns>Задача завершения остановки трассировки</returns>
    Task StopAsync(IBrowserContext context);
}