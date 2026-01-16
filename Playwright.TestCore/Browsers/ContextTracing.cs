using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Configurations;

namespace SkbKontur.Playwright.TestCore.Browsers;

public class ContextTracing(ITracingConfigurator tracingConfigurator) : IContextTracing
{
    public Task StartAsync(IBrowserContext context)
        => context.Tracing.StartAsync(tracingConfigurator.GetTracingStartOptions());

    public Task StopAsync(IBrowserContext context)
        => context.Tracing.StopAsync(tracingConfigurator.GetTracingStopOptions());
}