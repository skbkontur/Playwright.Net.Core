using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.POM.Abstractions;


namespace SkbKontur.Playwright.TestCore.Factories;

/// <summary>
/// Интерфейс фабрики для создания страниц (page objects).
/// Предоставляет различные способы создания типизированных страниц.
/// </summary>
public interface IPageFactory
{
    /// <summary>
    /// Создать обёртку страницы на основе существующего экземпляра IPage.
    /// </summary>
    /// <typeparam name="TPage">Тип страницы</typeparam>
    /// <param name="page">Экземпляр страницы Playwright</param>
    /// <returns>Созданная страница (page objects)</returns>
    TPage Create<TPage>(IPage page)
        where TPage : IWrapper<IPage>;

    /// <summary>
    /// Создать обёртку страницы на основе локатора.
    /// </summary>
    /// <typeparam name="TPage">Тип страницы</typeparam>
    /// <param name="wrapper">Обёртка локатора, содержащая страницу</param>
    /// <returns>Созданная страница (page objects)</returns>
    TPage Create<TPage>(IWrapper<ILocator> wrapper)
        where TPage : IWrapper<IPage>;

    /// <summary>
    /// Создать обёртку страницы с помощью функции получения страницы.
    /// </summary>
    /// <typeparam name="TPage">Тип страницы</typeparam>
    /// <param name="getPage">Функция для получения экземпляра страницы</param>
    /// <returns>Созданная страница (page objects)</returns>
    TPage Create<TPage>(Func<TPage> getPage)
        where TPage : IWrapper<IPage>;

    /// <summary>
    /// Асинхронно создать обёртку страницы с помощью функции получения страницы.
    /// </summary>
    /// <typeparam name="TPage">Тип страницы</typeparam>
    /// <param name="getPageAsync">Асинхронная функция для получения экземпляра страницы</param>
    /// <returns>Задача, возвращающая созданную страницу (page objects)</returns>
    Task<TPage> CreateAsync<TPage>(Func<Task<TPage>> getPageAsync)
        where TPage : IWrapper<IPage>;
}