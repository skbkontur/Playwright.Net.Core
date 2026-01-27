using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.POM.Abstractions;

namespace SkbKontur.Playwright.TestCore.Factories;

/// <summary>
/// Интерфейс фабрики для создания страниц (page objects).
/// Предоставляет различные способы создания типизированных страниц.
/// </summary>
public interface IPageFactory : IPageFactory<IPage> 
{
    /// <summary>
    /// Создать обёртку страницы на основе локатора.
    /// </summary>
    /// <typeparam name="TPage">Тип страницы</typeparam>
    /// <param name="locatorWrapper">Обёртка локатора, содержащая страницу</param>
    /// <returns>Созданная страница (page objects)</returns>
    TPage Create<TPage>(ILocatorWrapper<ILocator> locatorWrapper)
        where TPage : IPageWrapper<IPage>;

    /// <summary>
    /// Асинхронно создать обёртку страницы с помощью функции получения страницы Playwright.
    /// </summary>
    /// <typeparam name="TPage">Тип страницы</typeparam>
    /// <param name="getPageAsync">Асинхронная функция для получения экземпляра страницы Playwright</param>
    /// <returns>Задача, возвращающая созданную страницу (page objects)</returns>
    Task<TPage> CreateAsync<TPage>(Func<Task<IPage>> getPageAsync)
        where TPage : IPageWrapper<IPage>;
}