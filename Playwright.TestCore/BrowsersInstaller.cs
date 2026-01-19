using System;
using System.Collections.Generic;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore;

/// <summary>
/// Установщик браузеров Playwright.
/// Предоставляет удобный интерфейс для установки браузеров перед запуском тестов.
/// </summary>
public class BrowsersInstaller
{
    /// <summary>
    /// Установить указанные браузеры Playwright.
    /// </summary>
    /// <param name="browsers">Массив имён браузеров для установки (chromium, firefox, webkit и т.д.)</param>
    /// <exception cref="Exception">Выбрасывается, если установка завершилась с ошибкой</exception>
    public void Install(params string[] browsers)
    {
        List<string> installArgs = ["install"];
        installArgs.AddRange(browsers);
        var exitCode = Program.Main(installArgs.ToArray());
        if (exitCode != 0)
            throw new Exception($"Playwright exited with code {exitCode}");
    }
}