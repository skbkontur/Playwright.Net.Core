using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Auth;

public class AuthWithCacheStrategy(IAutentificator autentificator) : IAuthStrategy
{
    private static readonly object Lock = new();
    private static string? _cachedStorageState = null;
    private static bool isInitialized = false;

    public BrowserNewContextOptions GetOrCreateContextOptionsAsync()
    {
        var state = GetOrCreateStorageStateAsync();
        return new BrowserNewContextOptions { StorageState = state };
    }    
    
    public string? GetOrCreateStorageStateAsync()
    {
        if (isInitialized || _cachedStorageState != null)
        {
            return _cachedStorageState;
        }

        lock (Lock)
        {
            if (!isInitialized)
            {
                _cachedStorageState ??= autentificator.CreateStorageStateAsync().GetAwaiter().GetResult();
                isInitialized = true;
            }
        }

        return _cachedStorageState;
    }
}