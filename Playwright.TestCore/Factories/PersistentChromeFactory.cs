using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Configurations;

namespace SkbKontur.Playwright.TestCore.Factories;

/// <summary>
/// Фабрика для создания персистентных контекстов браузера Chrome.
/// Каждый контекст сохраняется в отдельной директории пользователя.
/// </summary>
/// <param name="playwrightFactory">Фабрика для получения экземпляра Playwright</param>
/// <param name="browserConfigurator">Конфигуратор параметров запуска браузера</param>
/// <param name="testInfoGetter">Провайдер информации о текущем тесте</param>
public class PersistentChromeFactory(
    IPlaywrightFactory playwrightFactory,
    IBrowserConfigurator browserConfigurator,
    ITestInfoGetter testInfoGetter)
    : IBrowserFactory
{
    /// <summary>
    /// Создать новый персистентный контекст браузера Chrome.
    /// Контекст будет сохранён в уникальной директории пользователя.
    /// </summary>
    /// <returns>Задача, возвращающая созданный персистентный контекст браузера</returns>
    public async Task<IBrowserContext> CreateAsync()
    {
        var pw = await playwrightFactory.GetPlaywrightAsync();
        var userDir = Path.GetFullPath($"{testInfoGetter.WorkDirectory}/{Guid.NewGuid()}");
        var browser = await pw.Chromium.LaunchPersistentContextAsync(
            userDir,
            browserConfigurator.GetLaunchPersistentContextOptions()
        );
        return browser;
    }
}