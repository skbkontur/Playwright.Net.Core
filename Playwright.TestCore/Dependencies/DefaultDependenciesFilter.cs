using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Kontur.Playwright.TestCore.Dependencies;

public class DefaultDependenciesFilter : IDependenciesFilter
{
    public IEnumerable<ParameterInfo> Apply(IEnumerable<ParameterInfo> getParameters)
    {
        return getParameters
            .Where(x => x.ParameterType != typeof(IPage))
            .Where(x => x.ParameterType != typeof(ILocator))
            .Where(x => x.ParameterType != typeof(Task<IPage>))
            .Where(x => x.ParameterType != typeof(Task<ILocator>))
            .Where(x => x.ParameterType != typeof(Func<ILocator>))
            .Where(x => x.ParameterType != typeof(Func<IPage>))            
            .Where(x => x.ParameterType != typeof(Func<Task<ILocator>>))
            .Where(x => x.ParameterType != typeof(Func<Task<IPage>>));
    }
}