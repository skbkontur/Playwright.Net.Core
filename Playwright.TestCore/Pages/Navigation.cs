using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.POM.Abstractions;
using SkbKontur.Playwright.TestCore.Factories;

namespace SkbKontur.Playwright.TestCore.Pages;

/// <summary>
/// Сервис управления навигацией браузера.
/// Предоставляет методы для перехода как по прямым ссылкам, так и на типизированные страницы (Page Objects),
/// инкапсулируя логику создания объектов страниц и ожидания их загрузки.
/// </summary>
/// <param name="pageGetter">Провайдер для получения текущего экземпляра IPage (Playwright).</param>
/// <param name="pageObjectsFactory">Фабрика для создания экземпляров Page Object.</param>
public class Navigation(IPageGetter pageGetter, IPageFactory pageObjectsFactory)
{
    private readonly Lazy<Task<IPage>> _page = new(pageGetter.GetAsync);
    
    /// <summary>
    /// Выполняет переход на страницу <typeparamref name="TPage"/>.
    /// </summary>
    /// <typeparam name="TPage">Тип Page Object.</typeparam>
    /// <param name="path">
    /// Относительный путь, который будет добавлен к URL страницы.
    /// <br/>Например, передача <c>"/123?type=info"</c> к странице с url <c>/products</c> приведет к переходу на <c>/products/123?type=info</c>.
    /// </param>
    /// <param name="options">Настройки перехода Playwright.</param>
    public async Task<TPage> GoToPageAsync<TPage>(string? path = null, PageGotoOptions? options = null)
        where TPage : IPageWrapper<IPage>
    {
        var page = await _page.Value;
        var pageObject = pageObjectsFactory.Create<TPage>(page);
        var pageUrl = pageObject.Url.TrimEnd('/');
        var fullUrl = $"{pageUrl}{path ?? string.Empty}";
        await page.GotoAsync(fullUrl, options);

        if (pageObject is ILoadable loadable)
        {
            await loadable.WaitLoadAsync();
        }

        return pageObject;
    }

    /// <summary>
    /// Выполняет прямой переход по указанному строковому URL.
    /// Используется для навигации, не привязанной к конкретному Page Object (например, внешние ссылки).
    /// </summary>
    /// <param name="url">Абсолютный или относительный URL для перехода.</param>
    /// <param name="options">Опции навигации Playwright (таймаут, wait strategy).</param>
    /// <returns>Текущий экземпляр <see cref="IPage"/> (Playwright interface).</returns>
    public async Task<IPage> GoToUrlAsync(string url, PageGotoOptions? options = null)
    {
        var browserPage = await _page.Value;
        await browserPage.GotoAsync(url, options);
        return browserPage;
    }
}