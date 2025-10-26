using System;
using Microsoft.Playwright;

namespace Kontur.Playwright.TestCore;

public class DriversInstaller
{
    public void Install()
    {
        var exitCode = Program.Main(["install", "--with-deps", "chromium", "firefox"]);
        if (exitCode != 0)
            throw new Exception($"Playwright exited with code {exitCode}");
    }
}