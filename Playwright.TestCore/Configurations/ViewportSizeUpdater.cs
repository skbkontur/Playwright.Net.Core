using System.Threading.Tasks;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Configurations;

/// <summary>
/// Модификатор настроек, который отключает фиксированный размер области просмотра (Viewport),
/// позволяя странице адаптироваться под размер окна браузера.
/// </summary>
public class ViewportSizeUpdater : IContextOptionsUpdater
{
    /// <summary>
    /// Устанавливает значение ViewportSize в NoViewport для использования всего доступного пространства окна.
    /// </summary>
    /// <param name="options">Объект настроек контекста браузера Playwright.</param>
    /// <remarks>Установка в NoViewport необходима для корректной работы режима --start-maximized</remarks>>
    public Task ExecuteAsync(BrowserNewContextOptions options)
    {
        options.ViewportSize = ViewportSize.NoViewport;
        return Task.CompletedTask;
    }
}