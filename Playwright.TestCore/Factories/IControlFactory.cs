using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.POM.Abstractions;
using SkbKontur.Playwright.TestCore.Collections;


namespace SkbKontur.Playwright.TestCore.Factories;

/// <summary>
/// Интерфейс фабрики для создания controls.
/// Предоставляет методы для создания отдельных элементов и коллекций элементов.
/// Позволяет реализовать паттерн POM
/// </summary>
public interface IControlFactory
{
    /// <summary>
    /// Создать контрол на основе локатора и data-tid атрибута.
    /// </summary>
    /// <typeparam name="TControl">Тип контрола</typeparam>
    /// <param name="locator">Базовый локатор</param>
    /// <param name="dataTid">Значение data-tid атрибута для поиска элемента</param>
    /// <returns>Созданный контрол</returns>
    TControl Create<TControl>(ILocator locator, string dataTid)
        where TControl : IWrapper<ILocator>;

    /// <summary>
    /// Создать контрол на основе страницы и data-tid атрибута.
    /// </summary>
    /// <typeparam name="TControl">Тип контрола</typeparam>
    /// <param name="page">Экземпляр страницы Playwright</param>
    /// <param name="dataTid">Значение data-tid атрибута для поиска элемента</param>
    /// <returns>Созданный контрол</returns>
    TControl Create<TControl>(IPage page, string dataTid)
        where TControl : IWrapper<ILocator>;

    /// <summary>
    /// Создать контрол с помощью функции получения локатора.
    /// </summary>
    /// <typeparam name="TControl">Тип контрола</typeparam>
    /// <param name="locator">Функция для получения локатора</param>
    /// <returns>Созданный контрол</returns>
    TControl Create<TControl>(Func<ILocator> locator)
        where TControl : IWrapper<ILocator>;

    /// <summary>
    /// Асинхронно создать коллекцию контролов на основе локатора и data-tid.
    /// </summary>
    /// <typeparam name="TControl">Тип контрола</typeparam>
    /// <param name="locator">Базовый локатор</param>
    /// <param name="dataTid">Значение data-tid атрибута для поиска элементов</param>
    /// <returns>Задача, возвращающая коллекцию контролов</returns>
    Task<IReadOnlyCollection<TControl>> CreateCollectionAsync<TControl>(ILocator locator, string dataTid)
        where TControl : IWrapper<ILocator>;

    /// <summary>
    /// Асинхронно создать коллекцию контролов с помощью функции получения локатора.
    /// </summary>
    /// <typeparam name="TControl">Тип контрола</typeparam>
    /// <param name="locator">Функция для получения локатора</param>
    /// <returns>Задача, возвращающая коллекцию контролов</returns>
    Task<IReadOnlyCollection<TControl>> CreateCollectionAsync<TControl>(Func<ILocator> locator)
        where TControl : IWrapper<ILocator>;

    /// <summary>
    /// Асинхронно создать коллекцию контролов с помощью асинхронной функции получения локаторов.
    /// </summary>
    /// <typeparam name="TControl">Тип контрола</typeparam>
    /// <param name="locator">Асинхронная функция для получения коллекции локаторов</param>
    /// <returns>Задача, возвращающая коллекцию контролов</returns>
    Task<IReadOnlyCollection<TControl>> CreateCollectionAsync<TControl>(
        Func<Task<IEnumerable<ILocator>>> locator)
        where TControl : IWrapper<ILocator>;

    /// <summary>
    /// Создать типизированную коллекцию элементов на основе локатора и data-tid атрибута.
    /// </summary>
    /// <typeparam name="TControl">Тип контрола</typeparam>
    /// <param name="locator">Базовый локатор</param>
    /// <param name="elementDataTid">Значение data-tid атрибута для поиска элементов</param>
    /// <returns>Созданная типизированная коллекция элементов</returns>
    ElementsCollection<TControl> CreateElementsCollection<TControl>(ILocator locator, string elementDataTid)
        where TControl : IWrapper<ILocator>;

    /// <summary>
    /// Создать типизированную коллекцию элементов на основе обёртки локатора и data-tid атрибута.
    /// </summary>
    /// <typeparam name="TControl">Тип контрола</typeparam>
    /// <param name="locator">Обёртка локатора</param>
    /// <param name="elementDataTid">Значение data-tid атрибута для поиска элементов</param>
    /// <returns>Созданная типизированная коллекция элементов</returns>
    ElementsCollection<TControl> CreateElementsCollection<TControl>(IWrapper<ILocator> locator, string elementDataTid)
        where TControl : IWrapper<ILocator>;

    /// <summary>
    /// Создать типизированную коллекцию элементов на основе обёртки страницы и data-tid атрибута.
    /// </summary>
    /// <typeparam name="TControl">Тип контрола</typeparam>
    /// <param name="page">Обёртка страницы</param>
    /// <param name="elementDataTid">Значение data-tid атрибута для поиска элементов</param>
    /// <returns>Созданная типизированная коллекция элементов</returns>
    ElementsCollection<TControl> CreateElementsCollection<TControl>(IWrapper<IPage> page, string elementDataTid)
        where TControl : IWrapper<ILocator>;

    /// <summary>
    /// Создать типизированную коллекцию элементов на основе страницы и data-tid атрибута.
    /// </summary>
    /// <typeparam name="TControl">Тип контрола</typeparam>
    /// <param name="page">Экземпляр страницы Playwright</param>
    /// <param name="elementDataTid">Значение data-tid атрибута для поиска элементов</param>
    /// <returns>Созданная типизированная коллекция элементов</returns>
    ElementsCollection<TControl> CreateElementsCollection<TControl>(IPage page, string elementDataTid)
        where TControl : IWrapper<ILocator>;

    /// <summary>
    /// Создать типизированную коллекцию элементов на основе локатора.
    /// </summary>
    /// <typeparam name="TControl">Тип контрола</typeparam>
    /// <param name="getElementLocator">Локатор для поиска элементов коллекции</param>
    /// <returns>Созданная типизированная коллекция элементов</returns>
    ElementsCollection<TControl> CreateElementsCollection<TControl>(
       ILocator getElementLocator)
        where TControl : IWrapper<ILocator>;
}