using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Browsers;

namespace SkbKontur.Playwright.TestCore.Pages;

/// <summary>
/// Реализация получателя активной страницы браузера.
/// Управляет жизненным циклом страницы в контексте браузера.
/// </summary>
/// <param name="browserGetter">Получатель контекста браузера</param>
public class PageGetter(IBrowserGetter browserGetter)
    : IPageGetter, IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Лениво инициализируемая страница браузера.
    /// При первом обращении получает существующую страницу или создаёт новую.
    /// </summary>
    private readonly Lazy<Task<IPage>> _page = new(
        async () =>
        {
            var context = await browserGetter.GetAsync();
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
        if (_page.IsValueCreated)
        {
            var page = await _page.Value;
            await page.CloseAsync();
        }
    }

    /// <summary>
    /// Закрыть страницу браузера.
    /// </summary>
    public void Dispose()
        => DisposeAsync().GetAwaiter().GetResult();
}