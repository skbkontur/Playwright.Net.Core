using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Configurations;

namespace SkbKontur.Playwright.TestCore.Factories;

/// <summary>
/// Фабрика для создания персистентных контекстов браузера Firefox.
/// Каждый контекст сохраняется в отдельной директории пользователя.
/// </summary>
/// <param name="playwrightGetter">Фабрика для получения экземпляра Playwright</param>
/// <param name="browserConfigurator">Конфигуратор параметров запуска браузера</param>
/// <param name="testInfoGetter">Провайдер информации о текущем тесте</param>
public class PersistentFirefoxContextFactory(
    IPlaywrightGetter playwrightGetter,
    IBrowserConfigurator browserConfigurator
) : IBrowserContextFactory
{
    /// <summary>
    /// Создать новый персистентный контекст браузера Firefox.
    /// Контекст будет сохранён в уникальной директории пользователя.
    /// </summary>
    /// <returns>Задача, возвращающая созданный персистентный контекст браузера</returns>
    public async Task<IBrowserContext> CreateAsync()
    {
        var pw = await playwrightGetter.GetPlaywrightAsync();
        var userDir = Path.GetFullPath($"{AppContext.BaseDirectory}/{Guid.NewGuid()}");
        var browser = await pw.Firefox.LaunchPersistentContextAsync(
            userDir,
            browserConfigurator.GetLaunchPersistentContextOptions()
        );
        return browser;
    }
}