using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Kontur.Playwright.TestCore.Factories;

public interface IBrowserFactory
{
    Task<IBrowserContext> CreateAsync();
}