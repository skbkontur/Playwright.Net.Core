using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.POM.Abstractions;

namespace SkbKontur.Playwright.TestCore.Factories;

public interface IPageFactory
{
    TPage Create<TPage>(IPage page)
        where TPage : IWrapper<IPage>;

    TPage Create<TPage>(IWrapper<ILocator> wrapper)
        where TPage : IWrapper<IPage>;

    TPage Create<TPage>(Func<TPage> getPage)
        where TPage : IWrapper<IPage>;

    Task<TPage> CreateAsync<TPage>(Func<Task<TPage>> getPageAsync)
        where TPage : IWrapper<IPage>;
}