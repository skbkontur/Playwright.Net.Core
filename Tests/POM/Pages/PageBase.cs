using Microsoft.Playwright;
using SkbKontur.Playwright.POM.Abstractions;

namespace Tests.POM.Pages;

public abstract class PageBase(IPage wrappedItem) : IPageWrapper<IPage>
{
    public IPage WrappedItem { get; } =  wrappedItem;
    public abstract string Url { get; }
}