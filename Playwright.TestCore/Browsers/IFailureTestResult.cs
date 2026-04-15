using System.Threading.Tasks;

namespace SkbKontur.Playwright.TestCore.Browsers;

/// <summary>
/// Интерфейс для определения результата выполнения теста.
/// Используется для условной записи трассировок только при падении тестов.
/// </summary>
public interface IFailureTestResult
{
    /// <summary>
    /// Проверить, завершился ли текущий тест с ошибкой.
    /// </summary>
    /// <returns>Задача, возвращающая true, если тест завершился с ошибкой</returns>
    Task<bool> IsFailureAsync();
}