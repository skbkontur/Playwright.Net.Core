using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Configurations;

namespace SkbKontur.Playwright.TestCore.Factories;

public class PersistentWebkitContextFactory(
    IPlaywrightGetter playwrightGetter,
    IBrowserConfigurator browserConfigurator
) : IBrowserContextFactory
{
    public async Task<IBrowserContext> CreateAsync()
    {
        var pw = await playwrightGetter.GetPlaywrightAsync();
        var userDir = Path.GetFullPath($"{AppContext.BaseDirectory}/{Guid.NewGuid()}");
        var browser = await pw.Webkit.LaunchPersistentContextAsync(
            userDir,
            browserConfigurator.GetLaunchPersistentContextOptions()
        );
        return browser;
    }
} 