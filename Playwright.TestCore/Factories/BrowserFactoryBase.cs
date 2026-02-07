using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Auth;
using SkbKontur.Playwright.TestCore.Configurations;

namespace SkbKontur.Playwright.TestCore.Factories;

/// <summary>
/// Абстрактная базовая фабрика для создания браузерных контекстов.
/// Определяет общий алгоритм создания контекста с применением стратегии аутентификации.
/// </summary>
/// <param name="playwrightFactory">Фабрика для получения экземпляра Playwright</param>
/// <param name="authStrategy">Стратегия аутентификации для применения к контексту браузера</param>
public abstract class BrowserFactoryBase(
    IPlaywrightFactory playwrightFactory,
    IAuthStrategy authStrategy,
    IContextOptionsUpdater contextUpdater
) : IBrowserFactory
{
    /// <summary>
    /// Создать новый контекст браузера с применением стратегии аутентификации.
    /// </summary>
    /// <returns>Задача, возвращающая созданный IBrowserContext</returns>
    public virtual async Task<IBrowserContext> CreateAsync()
    {
        var pw = await playwrightFactory.GetPlaywrightAsync();
        var browser = await LaunchAsync(pw);
        var storageState = await authStrategy.GetOrCreateStorageStateAsync();
        var options = new BrowserNewContextOptions
        {
            StorageState = storageState
        };
        contextUpdater.ExecuteAsync(options);
        return await browser.NewContextAsync(options);
    }

    /// <summary>
    /// Абстрактный метод для запуска конкретного браузера.
    /// Реализации должны определить, какой браузер запускать (Chrome, Firefox и т.д.).
    /// </summary>
    /// <param name="pw">Экземпляр Playwright</param>
    /// <returns>Задача, возвращающая запущенный браузер</returns>
    protected abstract Task<IBrowser> LaunchAsync(IPlaywright pw);
}