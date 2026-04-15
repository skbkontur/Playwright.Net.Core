using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Configurations;

namespace SkbKontur.Playwright.TestCore;

/// <summary>
/// Провайдер экземпляров Playwright с применением конфигурации.
/// Использует ленивую инициализацию для создания единственного экземпляра Playwright.
/// Реализует IDisposable и IAsyncDisposable для корректного освобождения ресурсов.
/// </summary>
/// <typeparam name="TConfiguration">Тип конфигурации Playwright, должен реализовывать IPlaywrightConfiguration</typeparam>
public class PlaywrightProvider<TConfiguration> : IPlaywrightGetter, IAsyncDisposable, IDisposable
    where TConfiguration : class, IPlaywrightConfiguration, new()
{
    /// <summary>
    /// Статическое поле для ленивой инициализации экземпляра Playwright.
    /// Гарантирует создание только одного экземпляра на домен приложения.
    /// </summary>
    private static readonly Lazy<Task<IPlaywright>> Playwright = new(async () =>
    {
        var pw = await Microsoft.Playwright.Playwright.CreateAsync();
        new TConfiguration().ApplyConfiguration(pw);
        return pw;
    });

    /// <summary>
    /// Получить экземпляр Playwright с применённой конфигурацией.
    /// </summary>
    /// <returns>Задача, возвращающая настроенный экземпляр IPlaywright</returns>
    public Task<IPlaywright> GetPlaywrightAsync()
        => Playwright.Value;

    /// <summary>
    /// Освободить ресурсы экземпляра Playwright.
    /// </summary>
    public void Dispose()
    {
        if (Playwright.IsValueCreated)
        {
            Playwright.Value.Dispose();
        }
    }

    /// <summary>
    /// Асинхронно освободить ресурсы экземпляра Playwright.
    /// </summary>
    public async ValueTask DisposeAsync()
        => await Task.Run(Dispose);
}