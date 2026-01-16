using System.Threading.Tasks;

namespace SkbKontur.Playwright.TestCore.Auth;

public class WithoutAuthAutentificator : IAutentificator
{
    public Task<string?> CreateStorageStateAsync()
        => Task.FromResult<string?>(null);
}