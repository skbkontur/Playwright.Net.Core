using NUnit.Framework;
using SkbKontur.Playwright.TestCore.Configurations;

namespace Tests.Infra;

public class TestInfoGetter : ITestInfoGetter
{
    public string TestId => TestContext.CurrentContext.Test.ID;
    public string TestName => TestContext.CurrentContext.Test.Name;
    public string TestClassName => TestContext.CurrentContext.Test.ClassName ?? "";
    public string WorkDirectory => TestContext.CurrentContext.WorkDirectory;
}