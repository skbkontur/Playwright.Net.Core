using System.Threading.Tasks;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Pages;

/// <summary>
/// Интерфейс для выполнения действий перед закрытием страницы.
/// Позволяет расширять поведение при завершении работы со страницей,
/// например, для сохранения скриншотов или сбора данных.
/// </summary>
public interface IBeforeDisposePageActions
{
    /// <summary>
    /// Выполнить действия над страницей перед её закрытием.
    /// </summary>
    /// <param name="page">Страница браузера, которая будет закрыта</param>
    /// <returns>Задача завершения действий</returns>
    Task ExecuteAsync(IPage page);
}