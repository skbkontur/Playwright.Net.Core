using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Playwright;
using SkbKontur.Playwright.POM.Abstractions;
using SkbKontur.Playwright.TestCore.Factories;


namespace SkbKontur.Playwright.TestCore.Collections;

/// <summary>
/// Типизированная коллекция элементов на странице.
/// Предоставляет LINQ-подобный интерфейс для работы с коллекциями элементов.
/// </summary>
/// <typeparam name="TItem">Тип обёртки элемента управления</typeparam>
/// <param name="locator">Локатор базовых элементов коллекции</param>
/// <param name="controlFactory">Фабрика для создания обёрток элементов управления</param>
public class ElementsCollection<TItem>(ILocator locator, IControlFactory controlFactory)
    : IEnumerable<TItem>, ILocatorWrapper<ILocator>
    where TItem : ILocatorWrapper<ILocator>
{
    /// <summary>
    /// Обёрнутый локатор базовых элементов коллекции.
    /// </summary>
    public ILocator WrappedItem { get; } = locator;

    /// <summary>
    /// Отфильтровать коллекцию элементов по заданным критериям.
    /// </summary>
    /// <param name="options">Параметры фильтрации элементов</param>
    /// <returns>Новая отфильтрованная коллекция элементов</returns>
    public ElementsCollection<TItem> Filter(LocatorFilterOptions options)
        => controlFactory.CreateElementsCollection<TItem>(WrappedItem.Filter(options));

    /// <summary>
    /// Отфильтровать коллекцию элементов на основе свойства дочернего элемента.
    /// </summary>
    /// <param name="getProp">Функция получения свойства элемента для фильтрации</param>
    /// <param name="options">Параметры фильтрации (опционально)</param>
    /// <returns>Новая отфильтрованная коллекция элементов</returns>
    public ElementsCollection<TItem> Filter(Func<TItem, IWrapper<ILocator>> getProp, LocatorFilterOptions? options)
    {
        var locatorWithFilter = getProp(controlFactory.Create<TItem>(() => WrappedItem))
            .WrappedItem.Filter(options);
        return controlFactory.CreateElementsCollection<TItem>(locatorWithFilter);
    }

    /// <summary>
    /// Получить первый элемент коллекции.
    /// </summary>
    public TItem First
        => controlFactory.Create<TItem>(() => WrappedItem.First);

    /// <summary>
    /// Получить последний элемент коллекции.
    /// </summary>
    public TItem Last
        => controlFactory.Create<TItem>(() => WrappedItem.Last);

    /// <summary>
    /// Получить элемент коллекции по индексу.
    /// </summary>
    /// <param name="index">Индекс элемента (начиная с 0)</param>
    /// <returns>Элемент с указанным индексом</returns>
    public TItem Nth(int index)
        => controlFactory.Create<TItem>(() => WrappedItem.Nth(index));

    /// <summary>
    /// Получить перечислитель для итерации по элементам коллекции.
    /// </summary>
    /// <returns>Перечислитель элементов коллекции</returns>
    private IEnumerator<TItem> GetEnumerator()
        => WrappedItem.AllAsync().GetAwaiter().GetResult()
            .Select(x => controlFactory.Create<TItem>(() => x))
            .GetEnumerator();

    /// <summary>
    /// Получить объект для выполнения утверждений (assertions) на коллекции.
    /// </summary>
    /// <returns>Объект для выполнения утверждений</returns>
    public ILocatorAssertions Expect()
        => Assertions.Expect(WrappedItem);

    /// <summary>
    /// Получить не-generic перечислитель для совместимости с IEnumerable.
    /// </summary>
    /// <returns>Не-generic перечислитель</returns>
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    /// <summary>
    /// Получить generic перечислитель для итерации по элементам коллекции.
    /// </summary>
    /// <returns>Generic перечислитель элементов</returns>
    IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
        => GetEnumerator();
}