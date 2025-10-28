using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Configurations;

public interface ITracingConfigurator
{
    TracingStartOptions GetTracingStartOptions();
    TracingStopOptions GetTracingStopOptions();
}