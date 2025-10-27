using System;
using System.Collections.Generic;
using Microsoft.Playwright;

namespace Kontur.Playwright.TestCore;

public class BrowsersInstaller
{
    public void Install(params string[] browsers)
    {
        List<string> installArgs = ["install", "--with-deps"];
        installArgs.AddRange(browsers);
        var exitCode = Program.Main(installArgs.ToArray());
        if (exitCode != 0)
            throw new Exception($"Playwright exited with code {exitCode}");
    }
}