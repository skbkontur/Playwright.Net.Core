using System;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Configurations;

/// <summary>
/// Конфигуратор браузера, который автоматически определяет режим headless.
/// В среде GitLab CI запускает браузер в headless режиме, иначе в обычном режиме.
/// </summary>
public class HeadlessConfigurator : IBrowserConfigurator
{
    /// <summary>
    /// Определяет, запущен ли тест в среде GitLab CI.
    /// </summary>
    private static bool IsGitlabCi
        => Environment.GetEnvironmentVariable("ENVIRONMENT") == "GitLabCI";

    /// <summary>
    /// Получить параметры запуска браузера с автоматическим определением headless режима.
    /// </summary>
    /// <returns>Параметры запуска браузера</returns>
    public BrowserTypeLaunchOptions GetLaunchOptions()
        => new() {Headless = IsGitlabCi};

    /// <summary>
    /// Получить параметры запуска браузера с персистентным контекстом и автоматическим определением headless режима.
    /// </summary>
    /// <returns>Параметры запуска браузера с персистентным контекстом</returns>
    public BrowserTypeLaunchPersistentContextOptions GetLaunchPersistentContextOptions()
        => new() {Headless = IsGitlabCi};
}