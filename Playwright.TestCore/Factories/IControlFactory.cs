using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.POM.Abstractions;
using SkbKontur.Playwright.TestCore.Collections;


namespace SkbKontur.Playwright.TestCore.Factories;

public interface IControlFactory
{
    TControl Create<TControl>(ILocator locator, string dataTid)
        where TControl : IWrapper<ILocator>;

    TControl Create<TControl>(IPage page, string dataTid)
        where TControl : IWrapper<ILocator>;

    TControl Create<TControl>(Func<ILocator> locator)
        where TControl : IWrapper<ILocator>;

    Task<IReadOnlyCollection<TControl>> CreateCollectionAsync<TControl>(ILocator locator, string dataTid)
        where TControl : IWrapper<ILocator>;

    Task<IReadOnlyCollection<TControl>> CreateCollectionAsync<TControl>(Func<ILocator> locator)
        where TControl : IWrapper<ILocator>;

    Task<IReadOnlyCollection<TControl>> CreateCollectionAsync<TControl>(
        Func<Task<IEnumerable<ILocator>>> locator)
        where TControl : IWrapper<ILocator>;

    ElementsCollection<TControl> CreateElementsCollection<TControl>(ILocator locator, string elementDataTid)
        where TControl : IWrapper<ILocator>;

    ElementsCollection<TControl> CreateElementsCollection<TControl>(IWrapper<ILocator> locator, string elementDataTid)
        where TControl : IWrapper<ILocator>;

    ElementsCollection<TControl> CreateElementsCollection<TControl>(IWrapper<IPage> page, string elementDataTid)
        where TControl : IWrapper<ILocator>;

    ElementsCollection<TControl> CreateElementsCollection<TControl>(IPage page, string elementDataTid)
        where TControl : IWrapper<ILocator>;

    ElementsCollection<TControl> CreateElementsCollection<TControl>(
       ILocator getElementLocator)
        where TControl : IWrapper<ILocator>;
}