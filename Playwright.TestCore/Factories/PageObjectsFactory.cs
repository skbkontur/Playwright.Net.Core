using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Kontur.Playwright.TestCore.Collections;
using Kontur.Playwright.TestCore.Dependencies;
using Kontur.POM.Abstractions;
using Microsoft.Playwright;

namespace Kontur.Playwright.TestCore.Factories;

public class PageObjectsFactory(IDependenciesFactory dependenciesFactory)
    : IControlFactory, IPageFactory, IPageObjectsFactory
{
    public TControl Create<TControl>(ILocator locator)
        where TControl : IWrapper<ILocator>
    {
        return Create<TControl>(() => locator);
    }

    public TControl Create<TControl>(Func<ILocator> locator)
        where TControl : IWrapper<ILocator>
    {
        var dependency = dependenciesFactory.CreateDependency(typeof(TControl));
        return (TControl)Activator.CreateInstance(typeof(TControl), new[] { locator() }.Concat(dependency).ToArray())!;
    }

    public TControl Create<TControl>(IPage page, string dataTid)
        where TControl : IWrapper<ILocator>
    {
        return Create<TControl>(() => page.GetByTestId(dataTid));
    }

    public TControl Create<TControl>(ILocator locator, string dataTid)
        where TControl : IWrapper<ILocator>
    {
        return Create<TControl>(() => locator.GetByTestId(dataTid));
    }

    public Task<IReadOnlyCollection<TControl>> CreateCollectionAsync<TControl>(ILocator locator, string dataTid)
        where TControl : IWrapper<ILocator>
    {
        return CreateCollectionAsync<TControl>(async () => await locator.GetByTestId(dataTid).AllAsync());
    }

    public Task<IReadOnlyCollection<TControl>> CreateCollectionAsync<TControl>(Func<ILocator> locator)
        where TControl : IWrapper<ILocator>
    {
        return CreateCollectionAsync<TControl>(async () => await locator().AllAsync());
    }

    public async Task<IReadOnlyCollection<TControl>> CreateCollectionAsync<TControl>(
        Func<Task<IEnumerable<ILocator>>> locator)
        where TControl : IWrapper<ILocator>
    {
        var controls = await locator();
        return new ReadOnlyCollection<TControl>(
            controls
                .Select(Create<TControl>)
                .ToList());
    }

    public ElementsCollection<TControl> CreateElementsCollection<TControl>(ILocator locator, string elementDataTid)
        where TControl : IWrapper<ILocator> =>
        CreateElementsCollection<TControl>(locator.GetByTestId(elementDataTid));

    public ElementsCollection<TControl> CreateElementsCollection<TControl>(IWrapper<ILocator> locator,
        string elementDataTid) where TControl : IWrapper<ILocator> =>
        CreateElementsCollection<TControl>(locator.WrappedItem.GetByTestId(elementDataTid));

    public ElementsCollection<TControl> CreateElementsCollection<TControl>(IWrapper<IPage> page, string elementDataTid)
        where TControl : IWrapper<ILocator> =>
        CreateElementsCollection<TControl>( page.WrappedItem.GetByTestId(elementDataTid));

    public ElementsCollection<TControl> CreateElementsCollection<TControl>(IPage page, string elementDataTid)
        where TControl : IWrapper<ILocator> =>
        CreateElementsCollection<TControl>(page.GetByTestId(elementDataTid));

    public ElementsCollection<TControl> CreateElementsCollection<TControl>(
        ILocator ElementLocator)
        where TControl : IWrapper<ILocator>
    {
        var dependency = dependenciesFactory.CreateDependency(typeof(ElementsCollection<TControl>));
        return (ElementsCollection<TControl>)Activator.CreateInstance(
            typeof(ElementsCollection<TControl>),
            new object[] { ElementLocator }.Concat(dependency).ToArray()
        )!;
    }

    public TPage Create<TPage>(IPage page)
        where TPage : IWrapper<IPage>
    {
        var dependency = dependenciesFactory.CreateDependency(typeof(TPage));
        return (TPage)Activator.CreateInstance(typeof(TPage), new[] { page }.Concat(dependency).ToArray())!;
    }

    public TPage Create<TPage>(IWrapper<ILocator> wrapper)
        where TPage : IWrapper<IPage> =>
        Create<TPage>(wrapper.WrappedItem.Page);

    public TPage Create<TPage>(Func<TPage> getPage)
        where TPage : IWrapper<IPage> =>
        Create<TPage>(getPage().WrappedItem);

    public async Task<TPage> CreateAsync<TPage>(Func<Task<TPage>> getPageAsync)
        where TPage : IWrapper<IPage>
    {
        var page = (await getPageAsync()).WrappedItem;
        return Create<TPage>(page);
    }

    public IPageFactory PageFactory => this;
    public IControlFactory ControlFactory => this;
}