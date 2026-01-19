namespace SkbKontur.Playwright.POM.Abstractions;

/// <summary>
/// Интерфейс для обёрток Playwright локаторов.
/// Наследуется от базового интерфейса IWrapper.
/// </summary>
/// <typeparam name="TLocator">Тип локатора Playwright (ILocator)</typeparam>
public interface ILocatorWrapper<out TLocator> : IWrapper<TLocator>;