using System;

namespace SkbKontur.Playwright.POM.Abstractions;

/// <summary>
/// Фабрика для создания страниц с поддержкой различных способов инициализации
/// </summary>
/// <typeparam name="TWrappedItem">Тип оборачиваемого элемента страницы</typeparam>
/// <remarks>
/// <para>
/// Интерфейс предоставляет единый механизм создания page objects с двумя стратегиями:
/// </para>
/// <list type="bullet">
/// <item><description>Создание по готовому объекту страницы</description></item>
/// <item><description>Создание c отложенной инициализацией</description></item>
/// </list>
/// <para>
/// Все создаваемые страницы реализуют интерфейс <see cref="IPageWrapper{TWrappedItem}"/>.
/// </para>
/// <para>
/// Используется в сочетании с Page Object Model для инкапсуляции логики работы со страницами.
/// </para>
/// </remarks>
public interface IPageFactory<in TWrappedItem>
{
    /// <summary>
    /// Создает page object с использованием готового объекта страницы
    /// </summary>
    /// <typeparam name="TPage">Тип создаваемой страницы, реализующий <see cref="IPageWrapper{TWrappedItem}"/></typeparam>
    /// <param name="page">Объект страницы для обертывания</param>
    /// <returns>Экземпляр page object указанного типа</returns>
    /// <remarks>
    /// Используется, когда объект страницы уже создан и нужно создать его обертку
    /// с дополнительной логикой и элементами управления
    /// </remarks>
    TPage Create<TPage>(TWrappedItem page)
        where TPage : IPageWrapper<TWrappedItem>;
    
    /// <summary>
    /// Создает page object с использованием метода получения объекта страницы
    /// </summary>
    /// <typeparam name="TPage">Тип создаваемой страницы, реализующий <see cref="IPageWrapper{TWrappedItem}"/></typeparam>
    /// <param name="getPage">Функция, возвращающая объект страницы при вызове</param>
    /// <returns>Экземпляр page object указанного типа</returns>
    /// <remarks>
    /// Полезен при необходимости отложенного создания страницы или когда
    /// страница должна быть создана в определенном контексте/состоянии
    /// </remarks>
    TPage Create<TPage>(Func<TWrappedItem> getPage)
        where TPage : IPageWrapper<TWrappedItem>;
}