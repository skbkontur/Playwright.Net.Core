using System.IO;
using Microsoft.Playwright;

namespace Kontur.Playwright.TestCore.Configurations;

public class DefaultTracingConfigurator(ITestInfoGetter infoGetter) : ITracingConfigurator
{
    public TracingStartOptions GetTracingStartOptions()
    {
        return new TracingStartOptions
        {
            Title = $"{infoGetter.TestClassName}.{infoGetter.TestName}",
            Screenshots = true,
            Snapshots = true,
            Sources = true
        };
    }

    public TracingStopOptions GetTracingStopOptions()
    {
        var path = Path.Combine(
            infoGetter.WorkDirectory,
            "playwright-traces",
            $"{infoGetter.TestClassName}.{infoGetter.TestName}.zip"
        );
        return new TracingStopOptions {Path = path};
    }
}