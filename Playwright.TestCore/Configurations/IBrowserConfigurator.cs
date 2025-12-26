using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Configurations;

public interface IBrowserConfigurator
{
    BrowserTypeLaunchOptions GetLaunchOptions();
    BrowserTypeLaunchPersistentContextOptions GetLaunchPersistentContextOptions();
}