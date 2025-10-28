using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Factories;
using SkbKontur.POM.Abstractions;

namespace SkbKontur.Playwright.TestCore.Collections;

public class ElementsCollection<TItem>(ILocator elementLocator, IControlFactory controlFactory) : IEnumerable<TItem>, IWrapper<ILocator>
    where TItem : IWrapper<ILocator>
{
    public ILocator WrappedItem { get; } = elementLocator;

    public ElementsCollection<TItem> Filter(LocatorFilterOptions options)
        => controlFactory.CreateElementsCollection<TItem>(WrappedItem.Filter(options));

    public ElementsCollection<TItem> Filter(Func<TItem, IWrapper<ILocator>> getProp, LocatorFilterOptions? options)
    {
        var locatorWithFilter = getProp(controlFactory.Create<TItem>(() => WrappedItem))
            .WrappedItem.Filter(options);
        return controlFactory.CreateElementsCollection<TItem>(locatorWithFilter);
    }

    public TItem First
        => controlFactory.Create<TItem>(() => WrappedItem.First);

    public TItem Last
        => controlFactory.Create<TItem>(() => WrappedItem.Last);

    public TItem Nth(int index)
        => controlFactory.Create<TItem>(() => WrappedItem.Nth(index));

    private IEnumerator<TItem> GetEnumerator()
        => WrappedItem.AllAsync().GetAwaiter().GetResult()
            .Select(x => controlFactory.Create<TItem>(() => x))
            .GetEnumerator();

    public ILocatorAssertions Expect()
        => Assertions.Expect(WrappedItem);
    
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
        => GetEnumerator();
}