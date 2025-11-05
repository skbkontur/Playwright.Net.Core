namespace SkbKontur.Playwright.POM.Abstractions;

public interface IWrapper<out T>
{
    T WrappedItem { get; }
}