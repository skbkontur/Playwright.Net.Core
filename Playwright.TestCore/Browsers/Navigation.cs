using System;
using System.Threading.Tasks;
using Kontur.Playwright.TestCore.Factories;
using Kontur.Playwright.TestCore.Pages;
using Kontur.POM.Abstractions;
using Microsoft.Playwright;

namespace Kontur.Playwright.TestCore.Browsers;

public class Navigation(IPageGetter pageGetter, IPageObjectsFactory pageObjectsFactory)
{
    private readonly Lazy<Task<IPage>> _page = new(pageGetter.GetAsync);

    public async Task<TPage> GoToPageAsync<TPage>(string? path = null, PageGotoOptions? options = null)
        where TPage : notnull, IPageWrapper<IPage>
    {
        var page = await _page.Value;
        var pageObject = pageObjectsFactory.PageFactory.Create<TPage>(page);

        await page.GotoAsync($"{pageObject.Url}/{path ?? string.Empty}", options);

        if (pageObject is ILoadable loadable)
        {
            await loadable.WaitLoadAsync();
        }

        return pageObject;
    }

    public async Task<IPage> GoToUrlAsync(string url, PageGotoOptions? options = null)
    {
        var browserPage = await _page.Value;
        await browserPage.GotoAsync(url, options);
        return browserPage;
    }
}