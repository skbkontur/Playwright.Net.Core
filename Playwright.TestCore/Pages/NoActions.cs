using System.Threading.Tasks;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Pages;

public class NoActions : IBeforeDisposePageActions
{
    public Task ExecuteAsync(IPage page)
        => Task.CompletedTask;
}