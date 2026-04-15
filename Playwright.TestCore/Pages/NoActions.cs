using System.Threading.Tasks;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Pages;

/// <summary>
/// Реализация <see cref="IBeforeDisposePageActions"/> без действий.
/// Используется по умолчанию, когда дополнительные действия перед закрытием страницы не требуются.
/// </summary>
public class NoActions : IBeforeDisposePageActions
{
    /// <summary>
    /// Не выполняет никаких действий.
    /// </summary>
    /// <param name="page">Страница браузера (не используется)</param>
    /// <returns>Завершённая задача</returns>
    public Task ExecuteAsync(IPage page)
        => Task.CompletedTask;
}