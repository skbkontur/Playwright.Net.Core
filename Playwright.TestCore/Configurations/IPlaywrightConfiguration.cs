using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Configurations;

public interface IPlaywrightConfiguration
{
    void ApplyConfiguration(IPlaywright pw);
}