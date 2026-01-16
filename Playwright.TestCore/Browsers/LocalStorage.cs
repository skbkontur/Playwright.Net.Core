using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Pages;

namespace SkbKontur.Playwright.TestCore.Browsers;

public class LocalStorage(IPageGetter pageGetter) : ILocalStorage, IAsyncDisposable, IDisposable
{
    private readonly Lazy<Task<IPage>> _page = new(pageGetter.GetAsync);

    public async Task<long> GetLengthAsync() 
        => await (await _page.Value).EvaluateAsync<long>("localStorage.length;");

    public async Task ClearAsync() 
        => await (await _page.Value).EvaluateAsync("localStorage.clear();");

    public async Task<string> GetItemAsync(string keyName) 
        => await (await _page.Value).EvaluateAsync<string>($"localStorage.getItem('{keyName}');");

    public async Task<string> GetKeyAsync(int keyNumber) 
        => await (await _page.Value).EvaluateAsync<string>($"localStorage.key({keyNumber});");

    public async Task RemoveItemAsync(string keyName) 
        => await (await _page.Value).EvaluateAsync($"localStorage.removeItem('{keyName}');");

    public async Task SetItemAsync(string keyName, string value) 
        => await (await _page.Value).EvaluateAsync($"localStorage.setItem(\"{keyName}\", '{value}');");

    public ValueTask DisposeAsync() 
        => new ValueTask(ClearAsync());

    public void Dispose()
        => DisposeAsync().GetAwaiter().GetResult();
}