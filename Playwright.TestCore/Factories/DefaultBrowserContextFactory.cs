using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Auth;
using SkbKontur.Playwright.TestCore.Browsers;
using SkbKontur.Playwright.TestCore.Configurations;

namespace SkbKontur.Playwright.TestCore.Factories;

/// <summary>
/// Фабрика для создания браузерных контекстов с применением стратегии аутентификации и обновления параметров контекста.
/// </summary>
/// <param name="browserGetter">Провайдер для получения экземпляра браузера</param>
/// <param name="authStrategy">Стратегия аутентификации для применения к контексту браузера</param>
/// <param name="contextUpdater">Обновитель параметров контекста браузера</param>
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