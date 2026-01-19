using System.Threading.Tasks;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Pages;

/// <summary>
/// Интерфейс для получения активной страницы браузера.
/// Определяет контракт для доступа к текущей странице в контексте браузера.
/// </summary>
public interface IPageGetter
{
    /// <summary>
    /// Асинхронно получить активную страницу браузера.
    /// Возвращает существующую страницу или создаёт новую, если страниц нет.
    /// </summary>
    /// <returns>Задача, возвращающая активную страницу браузера</returns>
    Task<IPage> GetAsync();
}