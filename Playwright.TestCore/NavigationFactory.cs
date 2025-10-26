using Kontur.Playwright.TestCore.Browsers;
using Kontur.Playwright.TestCore.Configurations;
using Kontur.Playwright.TestCore.Dependencies;
using Kontur.Playwright.TestCore.Factories;
using Kontur.Playwright.TestCore.Pages;

namespace Kontur.Playwright.TestCore;

public static class NavigationFactory
{
    public static Navigation Create(
        IDependenciesFactory dependenciesFactory,
        ITestInfoGetter testInfoGetter
    )
        => new Navigation(
            new PageGetter(new DefaultBrowserGetter(
                    new ChromeFactory(
                        new PlaywrightFactory<DefaultPlaywrightConfiguration>(),
                        new HeadlessConfigurator()
                    )),
                new DefaultTracingConfigurator(testInfoGetter)),
            new PageObjectsFactory(dependenciesFactory)
        );
}