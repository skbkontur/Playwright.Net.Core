using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.POM.Abstractions;
using SkbKontur.Playwright.TestCore.Collections;
using SkbKontur.Playwright.TestCore.Dependencies;


namespace SkbKontur.Playwright.TestCore.Factories;

public class PageObjectsFactory(IDependenciesFactory dependenciesFactory)
    : IControlFactory, IPageFactory, IPageObjectsFactory
{
    public TControl Create<TControl>(Func<ILocator> getLocator)
        where TControl : ILocatorWrapper<ILocator>
    {
        var dependency = dependenciesFactory.CreateDependency(typeof(TControl));
        var locator = getLocator();
        return (TControl)Activator.CreateInstance(typeof(TControl), new[] { locator }.Concat(dependency).ToArray())!;
    }

    public IPageFactory PageFactory => this;
    public IControlFactory ControlFactory => this;

    public TControl Create<TControl>(ILocator locator)
        where TControl : ILocatorWrapper<ILocator>
        => Create<TControl>(() => locator);

    public TControl Create<TControl>(ILocatorWrapper<ILocator> locatorWrapper, string dataTestId)
        where TControl : ILocatorWrapper<ILocator>
        => Create<TControl>(locatorWrapper.WrappedItem, dataTestId);

    public TControl Create<TControl>(ILocator locator, string dataTestId)
        where TControl : ILocatorWrapper<ILocator>
        => Create<TControl>(() => locator.GetByTestId(dataTestId));

    public TControl Create<TControl>(IPage page, string dataTestId)
        where TControl : ILocatorWrapper<ILocator>
        => Create<TControl>(() => page.GetByTestId(dataTestId));    
    
    public TControl Create<TControl>(IPageWrapper<IPage> page, string dataTestId)
        where TControl : ILocatorWrapper<ILocator>
        => Create<TControl>(() => page.WrappedItem.GetByTestId(dataTestId));

    public Task<IReadOnlyCollection<TControl>> CreateCollectionAsync<TControl>(ILocator locator, string dataTid)
        where TControl : ILocatorWrapper<ILocator>
        => CreateCollectionAsync<TControl>(async () => await locator.GetByTestId(dataTid).AllAsync());

    public Task<IReadOnlyCollection<TControl>> CreateCollectionAsync<TControl>(Func<ILocator> locator)
        where TControl : ILocatorWrapper<ILocator>
        => CreateCollectionAsync<TControl>(async () => await locator().AllAsync());

    public async Task<IReadOnlyCollection<TControl>> CreateCollectionAsync<TControl>(
        Func<Task<IEnumerable<ILocator>>> locator)
        where TControl : ILocatorWrapper<ILocator>
    {
        var controls = await locator();
        return new ReadOnlyCollection<TControl>(
            controls
                .Select(Create<TControl>)
                .ToList());
    }

    public ElementsCollection<TControl> CreateElementsCollection<TControl>(ILocator locator, string dataTestId)
        where TControl : ILocatorWrapper<ILocator>
        => CreateElementsCollection<TControl>(locator.GetByTestId(dataTestId));

    public ElementsCollection<TControl> CreateElementsCollection<TControl>(IWrapper<ILocator> locator, string dataTestId) 
        where TControl : ILocatorWrapper<ILocator>
        => CreateElementsCollection<TControl>(locator.WrappedItem.GetByTestId(dataTestId));

    public ElementsCollection<TControl> CreateElementsCollection<TControl>(IWrapper<IPage> page, string dataTestId)
        where TControl : ILocatorWrapper<ILocator>
        => CreateElementsCollection<TControl>(page.WrappedItem.GetByTestId(dataTestId));

    public ElementsCollection<TControl> CreateElementsCollection<TControl>(IPage page, string dataTestId)
        where TControl : ILocatorWrapper<ILocator>
        => CreateElementsCollection<TControl>(page.GetByTestId(dataTestId));

    public ElementsCollection<TControl> CreateElementsCollection<TControl>(ILocator locator)
        where TControl : ILocatorWrapper<ILocator>
        => Create<ElementsCollection<TControl>>(locator);

    public TPage Create<TPage>(Func<IPage> getPage)
        where TPage : IPageWrapper<IPage>
    {
        var page = getPage();
        var dependency = dependenciesFactory.CreateDependency(typeof(TPage));
        return (TPage)Activator.CreateInstance(typeof(TPage), new[] { page }.Concat(dependency).ToArray())!;
    }

    public TPage Create<TPage>(IPage page)
        where TPage : IPageWrapper<IPage>
        => Create<TPage>(() => page);

    public TPage Create<TPage>(ILocatorWrapper<ILocator> locatorWrapper)
        where TPage : IPageWrapper<IPage>
        => Create<TPage>(() => locatorWrapper.WrappedItem.Page);

    public async Task<TPage> CreateAsync<TPage>(Func<Task<IPage>> getPageAsync)
        where TPage : IPageWrapper<IPage>
    {
        var page = await getPageAsync();
        return Create<TPage>(page);
    }
}