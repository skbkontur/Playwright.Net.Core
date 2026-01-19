using System.Threading.Tasks;

namespace SkbKontur.Playwright.TestCore.Pages;

/// <summary>
/// Интерфейс для объектов, требующих асинхронной загрузки.
/// Определяет контракт для ожидания полной загрузки объекта.
/// </summary>
public interface ILoadable
{
    /// <summary>
    /// Асинхронно дождаться полной загрузки объекта.
    /// </summary>
    /// <returns>Задача завершения загрузки</returns>
    Task WaitLoadAsync();
}