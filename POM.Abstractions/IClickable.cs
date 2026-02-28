using System.Threading.Tasks;

namespace SkbKontur.Playwright.POM.Abstractions;

/// <summary>
/// Определяет контракт для интерактивных элементов UI, поддерживающих различные виды кликов 
/// и навигацию (переходы) к другим элементам или страницам.
/// </summary>
/// <typeparam name="TPage">Тип объекта, представляющего страницу (например, IPage в Playwright).</typeparam>
/// <typeparam name="TLocator">Тип объекта, представляющего локатор (например, ILocator в Playwright).</typeparam>
public interface IClickable<in TPage, in TLocator>
{
    /// <summary>
    /// Получает фабрику для создания оберток над элементами управления (контролами).
    /// Используется для инициализации возвращаемых объектов (модальных окон, сайдбаров).
    /// </summary>
    IControlFactory<TLocator> ControlFactory { get; }

    /// <summary>
    /// Получает фабрику для создания объектов страниц (Page Objects).
    /// Используется для инициализации возвращаемых страниц при навигации.
    /// </summary>
    IPageFactory<TPage> PageFactory { get; }

    /// <summary>
    /// Выполняет стандартный клик левой кнопкой мыши по элементу.
    /// </summary>
    Task ClickAsync();

    /// <summary>
    /// Кликает по элементу и ожидает открытия модального окна или диалога.
    /// </summary>
    /// <typeparam name="TControl">Тип ожидаемого элемента управления (должен реализовывать ILocatorWrapper).</typeparam>
    /// <returns>Инициализированный экземпляр элемента управления (модального окна).</returns>
    Task<TControl> ClickAndOpenModalAsync<TControl>() where TControl : ILocatorWrapper<TLocator>;

    /// <summary>
    /// Кликает по элементу и ожидает открытия боковой панели (Side Page/Drawer).
    /// </summary>
    /// <typeparam name="TControl">Тип ожидаемой боковой панели (должен реализовывать ILocatorWrapper).</typeparam>
    /// <returns>Инициализированный экземпляр боковой панели.</returns>
    Task<TControl> ClickAndOpenSidePageAsync<TControl>() where TControl : ILocatorWrapper<TLocator>;

    /// <summary>
    /// Кликает по элементу, инициируя переход на новую страницу (или обновление текущей), 
    /// и возвращает соответствующий Page Object.
    /// </summary>
    /// <typeparam name="TPageObj">Тип ожидаемого Page Object (должен реализовывать IPageWrapper).</typeparam>
    /// <returns>Инициализированный экземпляр новой страницы.</returns>
    Task<TPageObj> ClickAndOpenPageAsync<TPageObj>() where TPageObj : IPageWrapper<TPage>;

    /// <summary>
    /// Кликает по ссылке/кнопке, которая открывает **новую вкладку** (page/popup) в браузере.
    /// Ожидает события открытия страницы и возвращает её Page Object.
    /// </summary>
    /// <typeparam name="TPageObj">Тип Page Object для новой вкладки.</typeparam>
    /// <returns>Экземпляр Page Object для новой вкладки.</returns>
    Task<TPageObj> ClickAndOpenNewTabAsync<TPageObj>() where TPageObj : IPageWrapper<TPage>;
}