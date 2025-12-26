using System;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Configurations;

public class HeadlessConfigurator : IBrowserConfigurator
{
    private static bool IsGitlabCi
        => Environment.GetEnvironmentVariable("ENVIRONMENT") == "GitLabCI";

    public BrowserTypeLaunchOptions GetLaunchOptions() 
        => new() {Headless = IsGitlabCi};

    public BrowserTypeLaunchPersistentContextOptions GetLaunchPersistentContextOptions() 
        => new() {Headless = IsGitlabCi};
}