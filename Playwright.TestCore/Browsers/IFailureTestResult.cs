using System.Threading.Tasks;

namespace SkbKontur.Playwright.TestCore.Browsers;

public interface IFailureTestResult
{
    Task<bool> IsFailureAsync();
}