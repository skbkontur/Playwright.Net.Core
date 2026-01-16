using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Auth;

public interface IAuthStrategy
{
    BrowserNewContextOptions GetOrCreateContextOptionsAsync();
    string? GetOrCreateStorageStateAsync();
}