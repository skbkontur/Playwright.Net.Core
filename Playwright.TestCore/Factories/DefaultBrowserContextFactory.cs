using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Auth;
using SkbKontur.Playwright.TestCore.Configurations;

namespace SkbKontur.Playwright.TestCore.Factories;

/// <summary>
/// Абстрактная базовая фабрика для создания браузерных контекстов.
/// Определяет общий алгоритм создания контекста с применением стратегии аутентификации.
/// </summary>
/// <param name="browserGetter">Провайдер получения экземпляра Playwright</param>
/// <param name="authStrategy">Стратегия аутентификации для применения к контексту браузера</param>
/// <param name="contextUpdater">Дополнительные параметры для применения к контексту браузера</param>
public class DefaultBrowserContextFactory(
    IBrowserGetter browserGetter,
    IAuthStrategy authStrategy,
    IContextOptionsUpdater contextUpdater
) : IBrowserContextFactory
{
    /// <summary>
    /// Создать новый контекст браузера с применением стратегии аутентификации.
    /// </summary>
    /// <returns>Задача, возвращающая созданный IBrowserContext</returns>
    public virtual async Task<IBrowserContext> CreateAsync()
    {
        var browser = await browserGetter.GetAsync();
        var storageState = await authStrategy.GetOrCreateStorageStateAsync();
        var options = new BrowserNewContextOptions
        {
            StorageState = storageState
        };
        await contextUpdater.ExecuteAsync(options);
        return await browser.NewContextAsync(options);
    }
}