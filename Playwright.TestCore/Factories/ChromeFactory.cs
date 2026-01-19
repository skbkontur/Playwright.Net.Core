using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Auth;
using SkbKontur.Playwright.TestCore.Configurations;

namespace SkbKontur.Playwright.TestCore.Factories;

/// <summary>
/// Фабрика для создания контекстов браузера Chrome.
/// Использует Chromium движок Playwright с конфигурацией браузера.
/// </summary>
/// <param name="playwrightFactory">Фабрика для получения экземпляра Playwright</param>
/// <param name="browserConfigurator">Конфигуратор параметров запуска браузера</param>
/// <param name="authStrategy">Стратегия аутентификации для применения к контексту</param>
public class ChromeFactory(
    IPlaywrightFactory playwrightFactory,
    IBrowserConfigurator browserConfigurator,
    IAuthStrategy authStrategy
)
    : BrowserFactoryBase(playwrightFactory, authStrategy)
{
    /// <summary>
    /// Запустить браузер Chrome с указанными параметрами.
    /// </summary>
    /// <param name="pw">Экземпляр Playwright</param>
    /// <returns>Задача, возвращающая запущенный браузер Chrome</returns>
    protected override Task<IBrowser> LaunchAsync(IPlaywright pw)
        => pw.Chromium.LaunchAsync(browserConfigurator.GetLaunchOptions());
}