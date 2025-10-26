using System;
using System.IO;
using System.Threading.Tasks;
using Kontur.Playwright.TestCore.Configurations;
using Microsoft.Playwright;

namespace Kontur.Playwright.TestCore.Factories;

public class PersistentFirefoxFactory(
    IPlaywrightFactory playwrightFactory,
    IBrowserConfigurator browserConfigurator,
    ITestInfoGetter testInfoGetter)
    : IBrowserFactory
{
    public async Task<IBrowserContext> CreateAsync()
    {
        var pw = await playwrightFactory.GetPlaywrightAsync();
        var userDir = Path.GetFullPath($"{testInfoGetter.WorkDirectory}/{Guid.NewGuid()}");
        var browser = await pw.Firefox.LaunchPersistentContextAsync(
            userDir,
            browserConfigurator.GetLaunchPersistentContextOptions()
        );
        return browser;
    }
}