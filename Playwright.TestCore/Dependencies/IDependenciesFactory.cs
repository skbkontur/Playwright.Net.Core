using System;

namespace SkbKontur.Playwright.TestCore.Dependencies;

public interface IDependenciesFactory
{
    object[] CreateDependency(Type controlType);
}