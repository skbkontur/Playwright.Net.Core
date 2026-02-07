using System;

namespace SkbKontur.Playwright.TestCore.Configurations;

/// <summary>
/// Заглушка для <see cref="ITestInfoGetter"/>, возвращающая значения по умолчанию.
/// Используется в случаях, когда информация о текущем тесте недоступна или не требуется.
/// </summary>
public class EmptyTestInfoProvider : ITestInfoGetter
{
    public string TestId { get; } = $"Unknown_{nameof(TestId)}_{Guid.NewGuid()}";
    public string TestName { get; } = $"Unknown_{nameof(TestName)}_{Guid.NewGuid()}";
    public string TestClassName { get; } = $"Unknown_{nameof(TestClassName)}_{Guid.NewGuid()}";
    public string WorkDirectory { get; } = AppContext.BaseDirectory;
}