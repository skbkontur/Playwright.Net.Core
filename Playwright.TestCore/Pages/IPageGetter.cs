using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Kontur.Playwright.TestCore.Pages;

public interface IPageGetter
{
    Task<IPage> GetAsync();
}