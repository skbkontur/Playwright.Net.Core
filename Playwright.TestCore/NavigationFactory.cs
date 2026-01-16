using SkbKontur.Playwright.TestCore.Auth;
using SkbKontur.Playwright.TestCore.Browsers;
using SkbKontur.Playwright.TestCore.Configurations;
using SkbKontur.Playwright.TestCore.Dependencies;
using SkbKontur.Playwright.TestCore.Factories;
using SkbKontur.Playwright.TestCore.Pages;

namespace SkbKontur.Playwright.TestCore;

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
                    new HeadlessConfigurator(),
                    new WithoutAuthStrategy()
                    ),
                new ContextTracing(
                    new DefaultTracingConfigurator(testInfoGetter))
            )),
            new PageObjectsFactory(dependenciesFactory)
        );
}