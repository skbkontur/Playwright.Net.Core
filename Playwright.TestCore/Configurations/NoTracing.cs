using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Browsers;

namespace SkbKontur.Playwright.TestCore.Configurations;

public class NoTracing : IContextTracing
{
    public Task StartAsync(IBrowserContext context) => Task.CompletedTask;

    public Task StopAsync(IBrowserContext context) => Task.CompletedTask;
}