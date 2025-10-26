namespace Kontur.Playwright.TestCore.Factories;

public interface IPageObjectsFactory
{
    IPageFactory PageFactory { get; }
    IControlFactory ControlFactory { get; }
}