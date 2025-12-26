using System.Threading.Tasks;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Pages;

public interface IPageGetter
{
    Task<IPage> GetAsync();
}