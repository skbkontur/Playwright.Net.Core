using System;

namespace SkbKontur.Playwright.POM.Abstractions;

/// <summary>
/// Фабрика для создания контролов с поддержкой различных способов инициализации локаторов
/// </summary>
/// <typeparam name="TWrappedItem">Тип оборачиваемого элемента локатора</typeparam>
/// <remarks>
/// <para>
/// Интерфейс предоставляет единый механизм создания контролов с различными стратегиями:
/// </para>
/// <list type="bullet">
/// <item><description>Создание по готовому локатору</description></item>
/// <item><description>Создание с отложенной инициализацией</description></item>
/// <item><description>Создание с поиском по data-testid внутри родительского локатора</description></item>
/// </list>
/// <para>
/// Все создаваемые контролы реализуют интерфейс <see cref="ILocatorWrapper{TWrappedItem}"/>.
/// </para>
/// </remarks>
public interface IControlFactory<in TWrappedItem>
{
    /// <summary>
    /// Создает контрол с использованием готового локатора
    /// </summary>
    /// <typeparam name="TControl">Тип создаваемого контрола, реализующий <see cref="ILocatorWrapper{TWrappedItem}"/></typeparam>
    /// <param name="locator">Локатор для поиска элемента на странице</param>
    /// <returns>Экземпляр контрола указанного типа</returns>
    TControl Create<TControl>(TWrappedItem locator)
        where TControl : ILocatorWrapper<TWrappedItem>;

    /// <summary>
    /// Создает контрол с использованием функции для получения локатора
    /// </summary>
    /// <typeparam name="TControl">Тип создаваемого контрола, реализующий <see cref="ILocatorWrapper{TWrappedItem}"/></typeparam>
    /// <param name="getLocator">Функция, возвращающая локатор при вызове</param>
    /// <returns>Экземпляр контрола указанного типа</returns>
    /// <remarks>
    /// Полезен при необходимости отложенного создания локатора или когда локатор 
    /// зависит от динамических условий выполнения теста
    /// </remarks>
    TControl Create<TControl>(Func<TWrappedItem> getLocator)
        where TControl : ILocatorWrapper<TWrappedItem>;

    /// <summary>
    /// Создает контрол с поиском по data-testid
    /// </summary>
    /// <typeparam name="TControl">Тип создаваемого контрола, реализующий <see cref="ILocatorWrapper{TWrappedItem}"/></typeparam>
    /// <param name="locatorWrapper">Родительская обертка локатора для поиска внутри</param>
    /// <param name="dataTestId">Значение data-testid атрибута искомого элемента</param>
    /// <returns>Экземпляр контрола указанного типа</returns>
    /// <remarks>
    /// Используется для поиска элементов по data-testid атрибуту внутри родительского контейнера.
    /// Этот подход рекомендуется для стабильных селекторов в тестировании.
    /// </remarks>
    TControl Create<TControl>(ILocatorWrapper<TWrappedItem> locatorWrapper, string dataTestId)
        where TControl : ILocatorWrapper<TWrappedItem>;

    /// <summary>
    /// Создает контрол с поиском по data-testid внутри переданного локатора
    /// </summary>
    /// <typeparam name="TControl">Тип создаваемого контрола, реализующий <see cref="ILocatorWrapper{TWrappedItem}"/></typeparam>
    /// <param name="locator">Родительский локатор для поиска внутри</param>
    /// <param name="dataTestId">Значение data-testid атрибута искомого элемента</param>
    /// <returns>Экземпляр контрола указанного типа</returns>
    /// <remarks>
    /// Альтернативный вариант поиска по data-testid, использующий сырой локатор вместо обертки.
    /// Удобен при работе с необернутыми локаторами.
    /// </remarks>
    TControl Create<TControl>(TWrappedItem locator, string dataTestId)
        where TControl : ILocatorWrapper<TWrappedItem>;
}

