using System.Threading.Tasks;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Factories;

public interface IBrowserFactory
{
    Task<IBrowserContext> CreateAsync();
}