namespace SkbKontur.Playwright.TestCore.Configurations;

/// <summary>
/// Интерфейс для получения информации о текущем выполняемом тесте.
/// Предоставляет доступ к метаданным теста из фреймворка тестирования.
/// </summary>
public interface ITestInfoGetter
{
    /// <summary>
    /// Уникальный идентификатор теста
    /// </summary>
    string TestId { get; }

    /// <summary>
    /// Имя теста
    /// </summary>
    string TestName { get; }

    /// <summary>
    /// Имя класса, содержащего тест
    /// </summary>
    string TestClassName { get; }

    /// <summary>
    /// Рабочая директория для теста
    /// </summary>
    string WorkDirectory { get; }
}