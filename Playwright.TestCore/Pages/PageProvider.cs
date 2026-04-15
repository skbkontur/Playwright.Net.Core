using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Browsers;

namespace SkbKontur.Playwright.TestCore.Pages;

/// <summary>
/// Реализация получателя активной страницы браузера.
/// Управляет жизненным циклом страницы в контексте браузера.
/// </summary>
/// <param name="browserContextGetter">Получатель контекста браузера</param>
public class PageProvider(
    IBrowserContextGetter browserContextGetter,
    IEnumerable<IBeforeDisposePageActions> beforeDisposeActions
)
    : IPageGetter, IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Лениво инициализируемая страница браузера.
    /// При первом обращении получает существующую страницу или создаёт новую.
    /// </summary>
    private readonly Lazy<Task<IPage>> _page = new(async () =>
        {
            var context = await browserContextGetter.GetAsync();
            return context.Pages.FirstOrDefault() ?? await context.NewPageAsync();
        }
    );

    /// <summary>
    /// Получить активную страницу браузера.
    /// Возвращает существующую страницу или создаёт новую при первом обращении.
    /// </summary>
    /// <returns>Задача, возвращающая активную страницу браузера</returns>
    public Task<IPage> GetAsync()
        => _page.Value;

    /// <summary>
    /// Закрыть страницу браузера.
    /// </summary>
    /// <returns>Задача завершения закрытия страницы</returns>
    public async ValueTask DisposeAsync()
    {
        if (!_page.IsValueCreated)
            return;

        var task = _page.Value;
        if (task.IsFaulted || task.IsCanceled)
            return;

        try
        {
            var page = await task;
            foreach (var beforeDisposeAction in beforeDisposeActions)
            {
                await beforeDisposeAction.ExecuteAsync(page);
            }

            await page.CloseAsync();
        }
        catch (PlaywrightException)
        {
        }
    }

    /// <summary>
    /// Закрыть страницу браузера.
    /// </summary>
    public void Dispose()
        => DisposeAsync().GetAwaiter().GetResult();
}