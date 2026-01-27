using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.POM.Abstractions;

namespace SkbKontur.Playwright.TestCore.Collections;

/// <summary>
/// Расширения для работы с коллекциями элементов.
/// Предоставляют методы для выполнения утверждений на коллекциях элементов.
/// </summary>
public static class ElementsCollectionExtensions
{
    /// <summary>
    /// Проверить, что элементы коллекции содержат ожидаемый текст в правильном порядке.
    /// </summary>
    /// <typeparam name="TItem">Тип контрола</typeparam>
    /// <param name="elements">Коллекция элементов для проверки</param>
    /// <param name="expectedRowTexts">Ожидаемые тексты элементов в правильном порядке</param>
    /// <param name="options">Дополнительные параметры проверки текста</param>
    public static void ExpectToHaveText<TItem>(
        this IEnumerable<TItem> elements,
        IEnumerable<string> expectedRowTexts,
        LocatorAssertionsToHaveTextOptions? options = null
    ) where TItem : IWrapper<ILocator>
    {
        var assertions =
            elements.Zip<TItem, string, List<Task>>(
                expectedRowTexts,
                (actual, expect) => new List<Task>
                    { Assertions.Expect(actual.WrappedItem).ToHaveTextAsync(expect, options) }
            ).SelectMany(x => x).ToArray();
        Task.WaitAll(assertions);
    }

    /// <summary>
    /// Проверить, что двумерная коллекция элементов содержит ожидаемые тексты в правильном порядке.
    /// </summary>
    /// <typeparam name="TItem">Тип контрола</typeparam>
    /// <param name="elements">Двумерная коллекция элементов для проверки</param>
    /// <param name="expectedRowTexts">Ожидаемые тексты в виде двумерной коллекции</param>
    /// <param name="options">Дополнительные параметры проверки текста</param>
    public static void ExpectToHaveText<TItem>(
        this IEnumerable<IEnumerable<TItem>> elements,
        IEnumerable<IEnumerable<string>> expectedRowTexts,
        LocatorAssertionsToHaveTextOptions? options = null
    ) where TItem : IWrapper<ILocator>
    {
        var assertions = elements.ToWaitAssertionsWithOrder(expectedRowTexts, options);
        Task.WaitAll(assertions);
    }

    /// <summary>
    /// Асинхронно проверить, что коллекция элементов содержит ожидаемые тексты в правильном порядке.
    /// </summary>
    /// <typeparam name="TItem">Тип контрола</typeparam>
    /// <param name="elementsCollection">Коллекция элементов для проверки</param>
    /// <param name="getProps">Функция получения свойств элемента</param>
    /// <param name="expectedRowTexts">Ожидаемые тексты в виде двумерной коллекции</param>
    /// <param name="options">Дополнительные параметры проверки текста</param>
    public static async Task ExpectToHaveText<TItem>(
        this ElementsCollection<TItem> elementsCollection,
        Func<TItem, IEnumerable<IWrapper<ILocator>>> getProps,
        IEnumerable<IEnumerable<string>> expectedRowTexts,
        LocatorAssertionsToHaveTextOptions? options = null
    ) where TItem : ILocatorWrapper<ILocator>
    {
        await elementsCollection.Expect().ToHaveCountAsync(expectedRowTexts.Count());
        var assertions = elementsCollection
            .Select(getProps)
            .ToWaitAssertionsWithOrder(expectedRowTexts, options);
        Task.WaitAll(assertions);
    }

    /// <summary>
    /// Асинхронно проверить, что коллекция элементов содержит ожидаемые тексты без учёта порядка.
    /// </summary>
    /// <typeparam name="TItem">Тип контрола</typeparam>
    /// <param name="elementsCollection">Коллекция элементов для проверки</param>
    /// <param name="getProps">Функция получения свойств элемента</param>
    /// <param name="expectedRowTexts">Ожидаемые тексты в виде двумерной коллекции</param>
    /// <param name="options">Дополнительные параметры проверки текста</param>
    public static async Task ExpectToHaveTextWithoutOrder<TItem>(
        this ElementsCollection<TItem> elementsCollection,
        Func<TItem, IEnumerable<IWrapper<ILocator>>> getProps,
        IEnumerable<IEnumerable<string>> expectedRowTexts,
        LocatorAssertionsToHaveTextOptions? options = null
    ) where TItem : ILocatorWrapper<ILocator>
    {
        await elementsCollection.Expect().ToHaveCountAsync(expectedRowTexts.Count());
        var assertions = new List<Task>();
        foreach (var row in expectedRowTexts.ToList())
        {
            assertions.AddRange(
                elementsCollection
                    .Filter(new LocatorFilterOptions { HasText = row.First() })
                    .Select(getProps)
                    .ToWaitAssertionsWithOrder(new[] { row }, options)
            );
        }
        Task.WaitAll(assertions.ToArray());
    }

    /// <summary>
    /// Вспомогательный метод для создания массива задач утверждений с учётом порядка.
    /// </summary>
    /// <typeparam name="TItem">Тип контрола</typeparam>
    /// <param name="elements">Двумерная коллекция элементов</param>
    /// <param name="expectedRowTexts">Ожидаемые тексты</param>
    /// <param name="options">Параметры проверки текста</param>
    /// <returns>Массив задач утверждений</returns>
    private static Task[] ToWaitAssertionsWithOrder<TItem>(
        this IEnumerable<IEnumerable<TItem>> elements,
        IEnumerable<IEnumerable<string>> expectedRowTexts,
        LocatorAssertionsToHaveTextOptions? options = null
    ) where TItem : IWrapper<ILocator>
    {
        var assertions =
            elements.Zip(
                expectedRowTexts, (actual, expect) =>
                    actual.Zip(expect, (item, text) => Assertions.Expect(item.WrappedItem).ToHaveTextAsync(text, options))
            ).SelectMany(x => x).ToArray();
        return assertions;
    }
}