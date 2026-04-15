using System.Threading.Tasks;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Browsers;

public interface IBrowserGetter
{
    Task<IBrowser> GetAsync();
}