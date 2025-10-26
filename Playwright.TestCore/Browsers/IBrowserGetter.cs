using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Kontur.Playwright.TestCore.Browsers;

public interface IBrowserGetter
{
    Task<IBrowserContext> GetAsync();
}