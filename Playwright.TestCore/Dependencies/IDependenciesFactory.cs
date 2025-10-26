using System;

namespace Kontur.Playwright.TestCore.Dependencies;

public interface IDependenciesFactory
{
    object[] CreateDependency(Type controlType);
}