using Microsoft.Playwright;
using SkbKontur.Playwright.POM.Abstractions;

namespace Tests.POM.Controls;

public abstract class ControlBase(ILocator locator) : ILocatorWrapper<ILocator>
{
    public ILocator WrappedItem { get; } = locator;
    
    public ILocatorAssertions Expect()
        => Assertions.Expect(WrappedItem);
}