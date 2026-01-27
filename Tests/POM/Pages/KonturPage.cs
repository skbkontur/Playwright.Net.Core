using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Factories;
using Tests.POM.Controls;
using Header = Tests.POM.Controls.Header;

namespace Tests.POM.Pages;

public class KonturPage(IPage wrappedItem, IControlFactory controlFactory) : PageBase(wrappedItem)
{
    public override string Url { get; } = "https://kontur.ru";

    public Logo Logo
        => controlFactory.Create<Logo>(WrappedItem.Locator(".kontur-logo_main"));

    public Header EcosystemHeader
        => controlFactory.Create<Header>(WrappedItem.Locator("h1", new() { HasText = "Экосистема" }));
}