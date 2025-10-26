using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kontur.POM.Abstractions;
using Microsoft.Playwright;

namespace Kontur.Playwright.TestCore.Collections;

public static class ElementsCollectionExtensions
{
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

    public static void ExpectToHaveText<TItem>(
        this IEnumerable<IEnumerable<TItem>> elements,
        IEnumerable<IEnumerable<string>> expectedRowTexts,
        LocatorAssertionsToHaveTextOptions? options = null
    ) where TItem : IWrapper<ILocator>
    {
        var assertions = elements.ToWaitAssertionsWithOrder(expectedRowTexts, options);
        Task.WaitAll(assertions);
    }

    public static async Task ExpectToHaveText<TItem>(
        this ElementsCollection<TItem> elementsCollection,
        Func<TItem, IEnumerable<IWrapper<ILocator>>> getProps,
        IEnumerable<IEnumerable<string>> expectedRowTexts,
        LocatorAssertionsToHaveTextOptions? options = null
    ) where TItem : IWrapper<ILocator>
    {
        await elementsCollection.Expect().ToHaveCountAsync(expectedRowTexts.Count());
        var assertions = elementsCollection
            .Select(getProps)
            .ToWaitAssertionsWithOrder(expectedRowTexts, options);
        Task.WaitAll(assertions);
    }

    public static async Task ExpectToHaveTextWithoutOrder<TItem>(
        this ElementsCollection<TItem> elementsCollection,
        Func<TItem, IEnumerable<IWrapper<ILocator>>> getProps,
        IEnumerable<IEnumerable<string>> expectedRowTexts,
        LocatorAssertionsToHaveTextOptions? options = null
    ) where TItem : IWrapper<ILocator>
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