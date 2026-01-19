using NUnit.Framework;
using SkbKontur.Playwright.TestCore.Configurations;

namespace Tests.Infra;

/// <summary>
/// Провайдер информации о текущем тесте для интеграции с NUnit.
/// Получает метаданные теста из TestContext NUnit.
/// </summary>
public class TestInfoGetter : ITestInfoGetter
{
    /// <summary>
    /// Уникальный идентификатор теста из NUnit
    /// </summary>
    public string TestId => TestContext.CurrentContext.Test.ID;

    /// <summary>
    /// Имя теста из NUnit
    /// </summary>
    public string TestName => TestContext.CurrentContext.Test.Name;

    /// <summary>
    /// Имя класса теста из NUnit
    /// </summary>
    public string TestClassName => TestContext.CurrentContext.Test.ClassName ?? "";

    /// <summary>
    /// Рабочая директория теста из NUnit
    /// </summary>
    public string WorkDirectory => TestContext.CurrentContext.WorkDirectory;
}