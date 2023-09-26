using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrastructure;
internal static class BackendCompositionRoot
{
    private static IServiceProvider _serviceProvider;

    internal static void SetContainer(IServiceProvider container)
    {
        _serviceProvider = container;
    }

    internal static IServiceScope CreateScope()
    {
        return _serviceProvider.CreateScope();
    }
}
