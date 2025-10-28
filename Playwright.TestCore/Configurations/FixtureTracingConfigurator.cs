using System.IO;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Configurations;

public class FixtureTracingConfigurator(ITestInfoGetter infoGetter) : ITracingConfigurator
{
    public TracingStartOptions GetTracingStartOptions()
    {
        return new TracingStartOptions
        {
            Title = $"{infoGetter.TestClassName}",
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
            $"{infoGetter.TestClassName}.zip"
        );
        return new TracingStopOptions {Path = path};
    }
}