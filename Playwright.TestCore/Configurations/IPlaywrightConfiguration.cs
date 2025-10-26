using Microsoft.Playwright;

namespace Kontur.Playwright.TestCore.Configurations;

public interface IPlaywrightConfiguration
{
    void ApplyConfiguration(IPlaywright pw);
}