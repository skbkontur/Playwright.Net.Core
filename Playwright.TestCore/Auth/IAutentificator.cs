using System.Threading.Tasks;

namespace SkbKontur.Playwright.TestCore.Auth;

public interface IAutentificator
{
    Task<string?> CreateStorageStateAsync();
}