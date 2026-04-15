using System.Threading.Tasks;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Pages;

public interface IBeforeDisposePageActions
{
    Task ExecuteAsync(IPage page);
}