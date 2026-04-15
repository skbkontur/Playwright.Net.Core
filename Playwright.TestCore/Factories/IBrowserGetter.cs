using System.Threading.Tasks;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Factories;

public interface IBrowserGetter
{
    Task<IBrowser> GetAsync();
}