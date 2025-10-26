using System.Threading.Tasks;

namespace Kontur.Playwright.TestCore.Browsers;

public interface ILocalStorage
{
    Task<long> GetLengthAsync();
    Task ClearAsync();
    Task<string> GetItemAsync(string keyName);
    Task<string> GetKeyAsync(int keyNumber);
    Task RemoveItemAsync(string keyName);
    Task SetItemAsync(string keyName, string value);
}