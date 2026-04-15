using System.Threading.Tasks;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Factories;

/// <summary>
/// Интерфейс фабрики для создания браузеров.
/// </summary>
public interface IBrowserFactory
{
    /// <summary>
    /// Асинхронно создать новый браузер.
    /// </summary>
    /// <returns>Задача, возвращающая созданный IBrowser</returns>
    Task<IBrowser> CreateAsync();
}