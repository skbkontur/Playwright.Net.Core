using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Kontur.Playwright.TestCore.Factories;

public interface IPlaywrightFactory
{
    Task<IPlaywright> GetPlaywrightAsync();
}