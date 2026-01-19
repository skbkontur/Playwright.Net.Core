using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Auth;
using SkbKontur.Playwright.TestCore.Configurations;

namespace SkbKontur.Playwright.TestCore.Factories;

/// <summary>
/// Фабрика для создания контекстов браузера Firefox.
/// Использует Firefox движок Playwright с конфигурацией браузера.
/// </summary>
/// <param name="playwrightFactory">Фабрика для получения экземпляра Playwright</param>
/// <param name="browserConfigurator">Конфигуратор параметров запуска браузера</param>
/// <param name="authStrategy">Стратегия аутентификации для применения к контексту</param>
public class FirefoxFactory(
    IPlaywrightFactory playwrightFactory,
    IBrowserConfigurator browserConfigurator,
    IAuthStrategy authStrategy)
    : BrowserFactoryBase(playwrightFactory, authStrategy)
{
    /// <summary>
    /// Запустить браузер Firefox с указанными параметрами.
    /// </summary>
    /// <param name="pw">Экземпляр Playwright</param>
    /// <returns>Задача, возвращающая запущенный браузер Firefox</returns>
    protected override Task<IBrowser> LaunchAsync(IPlaywright pw)
        => pw.Firefox.LaunchAsync(browserConfigurator.GetLaunchOptions());
}