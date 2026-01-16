using System.Threading.Tasks;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Browsers;

public interface IContextTracing
{
    Task StartAsync(IBrowserContext context);
    Task StopAsync(IBrowserContext context);
}