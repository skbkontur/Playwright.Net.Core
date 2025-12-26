using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using SkbKontur.Playwright.TestCore.Configurations;

namespace SkbKontur.Playwright.TestCore.Factories;

public class PlaywrightFactory<TConfiguration> : IPlaywrightFactory
    where TConfiguration : class, IPlaywrightConfiguration, new()
{
    private static readonly Lazy<Task<IPlaywright>> Playwright = new(async () =>
    {
        var pw = await Microsoft.Playwright.Playwright.CreateAsync();
        new TConfiguration().ApplyConfiguration(pw);
        return pw;
    });

    public Task<IPlaywright> GetPlaywrightAsync()
        => Playwright.Value;
}