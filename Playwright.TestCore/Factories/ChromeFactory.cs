using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Configurations;

namespace SkbKontur.Playwright.TestCore.Factories;

/// <summary>
/// Фабрика для создания браузера Chrome (Chromium).
/// Использует Chromium движок Playwright с конфигурацией браузера.
/// </summary>
/// <param name="playwrightGetter">Провайдер для получения экземпляра Playwright</param>
/// <param name="configurator">Конфигуратор параметров запуска браузера</param>
public class ChromeFactory(IPlaywrightGetter playwrightGetter, IBrowserConfigurator configurator)
    : IBrowserFactory
{
    /// <summary>
    /// Создать и запустить браузер Chrome с указанными параметрами.
    /// </summary>
    /// <returns>Задача, возвращающая запущенный браузер Chrome</returns>
    public async Task<IBrowser> CreateAsync()
    {
        var pw = await playwrightGetter.GetPlaywrightAsync();
        return await pw.Chromium.LaunchAsync(configurator.GetLaunchOptions());
    }
}