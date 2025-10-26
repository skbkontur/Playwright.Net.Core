using Microsoft.Playwright;

namespace Kontur.Playwright.TestCore.Configurations;

public class DefaultPlaywrightConfiguration : IPlaywrightConfiguration
{
    public void ApplyConfiguration(IPlaywright pw)
    {
        pw.Selectors.SetTestIdAttribute("data-tid");
    }
}