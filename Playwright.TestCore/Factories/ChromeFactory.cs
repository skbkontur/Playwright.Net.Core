using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Auth;
using SkbKontur.Playwright.TestCore.Configurations;

namespace SkbKontur.Playwright.TestCore.Factories;

public class ChromeFactory(
    IPlaywrightFactory playwrightFactory,
    IBrowserConfigurator browserConfigurator,
    IAuthStrategy authStrategy
)
    : BrowserFactoryBase(playwrightFactory, authStrategy)
{
    protected override Task<IBrowser> LaunchAsync(IPlaywright pw)
        => pw.Chromium.LaunchAsync(browserConfigurator.GetLaunchOptions());
}