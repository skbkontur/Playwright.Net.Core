using System.Collections.Generic;
using System.Reflection;

namespace Kontur.Playwright.TestCore.Dependencies;

public interface IDependenciesFilter
{
    IEnumerable<ParameterInfo> Apply(IEnumerable<ParameterInfo> getParameters);
}