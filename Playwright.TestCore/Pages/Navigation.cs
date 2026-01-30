using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.POM.Abstractions;
using SkbKontur.Playwright.TestCore.Factories;

namespace SkbKontur.Playwright.TestCore.Pages;

public class Navigation(IPageGetter pageGetter, IPageFactory pageObjectsFactory)
{
    private readonly Lazy<Task<IPage>> _page = new(pageGetter.GetAsync);

    public async Task<TPage> GoToPageAsync<TPage>(string? path = null, PageGotoOptions? options = null)
        where TPage : notnull, IPageWrapper<IPage>
    {
        var page = await _page.Value;
        var pageObject = pageObjectsFactory.Create<TPage>(page);
        var pageUrl = pageObject.Url.TrimEnd('/');
        var fullUrl = $"{pageUrl}/{path ?? string.Empty}".TrimEnd('/');
        await page.GotoAsync(fullUrl, options);

        if (pageObject is ILoadable loadable)
        {
            await loadable.WaitLoadAsync();
        }

        return pageObject;
    }

    public async Task<IPage> GoToUrlAsync(string url, PageGotoOptions? options = null)
    {
        var browserPage = await _page.Value;
        var pageUrl = url.TrimEnd('/');
        await browserPage.GotoAsync(pageUrl, options);
        return browserPage;
    }
}