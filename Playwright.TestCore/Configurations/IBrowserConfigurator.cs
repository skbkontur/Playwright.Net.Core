using Microsoft.Playwright;

namespace Kontur.Playwright.TestCore.Configurations;

public interface IBrowserConfigurator
{
    BrowserTypeLaunchOptions GetLaunchOptions();
    BrowserTypeLaunchPersistentContextOptions GetLaunchPersistentContextOptions();
}