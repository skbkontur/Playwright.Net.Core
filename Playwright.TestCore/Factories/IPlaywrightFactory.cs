using System.Threading.Tasks;
using Microsoft.Playwright;

namespace SkbKontur.Playwright.TestCore.Factories;

public interface IPlaywrightFactory
{
    Task<IPlaywright> GetPlaywrightAsync();
}