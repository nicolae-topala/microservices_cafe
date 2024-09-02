using System.Reflection;

namespace MicroservicesCafe.Products.Shared;

public static class DependencyInjection
{
    public static readonly Assembly currentAssembly = typeof(DependencyInjection).Assembly;

}

