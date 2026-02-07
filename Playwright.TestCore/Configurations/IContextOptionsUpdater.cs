using System.Threading.Tasks;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Configurations;

/// <summary>
/// Определяет интерфейс для динамического обновления настроек контекста браузера.
/// </summary>
public interface IContextOptionsUpdater
{
    /// <summary>
    /// Выполняет модификацию переданного объекта настроек контекста.
    /// </summary>
    /// <param name="options">Объект настроек контекста браузера Playwright.</param>
    Task ExecuteAsync(BrowserNewContextOptions options);
}