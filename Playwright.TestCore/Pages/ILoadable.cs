using System.Threading.Tasks;

namespace SkbKontur.Playwright.TestCore.Pages;

public interface ILoadable
{
    Task WaitLoadAsync();
}