using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Auth;

public class WithoutAuthStrategy : IAuthStrategy
{
    public BrowserNewContextOptions GetOrCreateContextOptionsAsync()
        => new BrowserNewContextOptions();

    public string? GetOrCreateStorageStateAsync()
        => null;
}