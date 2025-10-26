using Microsoft.Playwright;

namespace Kontur.Playwright.TestCore.Configurations;

public interface ITracingConfigurator
{
    TracingStartOptions GetTracingStartOptions();
    TracingStopOptions GetTracingStopOptions();
}