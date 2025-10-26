using System.Threading.Tasks;

namespace Kontur.Playwright.TestCore.Pages;

public interface ILoadable
{
    Task WaitLoadAsync();
}