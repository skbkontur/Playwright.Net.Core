# Библиотека для унификации базовых сущностей необходимых для реализации паттернов PageObjects и PageElements.

Имеет 3 абстракции:

- [IWrapper<T>](IWrapper.cs) - обобщенный интерфейс для реализации простого паттерна Wrapper.
- [IPageWrapper<T>](IPageWrapper.cs) - для описания PageObject.
  ```csharp
  /// Пример для Playwright
  public abstract class PageBase : IPageWrapper<IPage>
  {
  }
  
  /// Пример для Selenium
  public abstract class PageBase : IPageWrapper<IWebDriver>
  {
  }
  ```
- [ILocatorWrapper<T>](ILocatorWrapper.cs) - для описания PageElements

  ```csharp
  /// Пример для Playwright
  public abstract class ControlBase : ILocatorWrapper<ILocator>
  {
  }
  
  /// Пример для Selenium
  public abstract class ControlBase : ILocatorWrapper<IWebElement>
  {
  }
  ```