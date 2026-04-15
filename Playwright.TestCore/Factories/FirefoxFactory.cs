using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Configurations;

namespace SkbKontur.Playwright.TestCore.Factories;

/// <summary>
/// Фабрика для создания браузера Firefox.
/// Использует Firefox движок Playwright с конфигурацией браузера.
/// </summary>
/// <param name="playwrightGetter">Провайдер для получения экземпляра Playwright</param>
/// <param name="configurator">Конфигуратор параметров запуска браузера</param>
public class FirefoxFactory(IPlaywrightGetter playwrightGetter, IBrowserConfigurator configurator)
    : IBrowserFactory
{
    /// <summary>
    /// Создать и запустить браузер Firefox с указанными параметрами.
    /// </summary>
    /// <returns>Задача, возвращающая запущенный браузер Firefox</returns>
    public async Task<IBrowser> CreateAsync()
    {
        var pw = await playwrightGetter.GetPlaywrightAsync();
        return await pw.Firefox.LaunchAsync(configurator.GetLaunchOptions());
    }
}