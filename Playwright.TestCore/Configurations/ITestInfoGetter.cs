namespace SkbKontur.Playwright.TestCore.Configurations;

public interface ITestInfoGetter
{
    string TestId { get; }
    string TestName { get; }
    string TestClassName { get; }
    string WorkDirectory { get; }
}