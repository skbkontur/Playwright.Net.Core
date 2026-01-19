using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Configurations;

namespace SkbKontur.Playwright.TestCore.Factories;

/// <summary>
/// Фабрика для создания экземпляров Playwright с применением конфигурации.
/// Использует ленивую инициализацию для создания единственного экземпляра Playwright.
/// </summary>
/// <typeparam name="TConfiguration">Тип конфигурации Playwright, должен реализовывать IPlaywrightConfiguration</typeparam>
public class PlaywrightFactory<TConfiguration> : IPlaywrightFactory
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
}