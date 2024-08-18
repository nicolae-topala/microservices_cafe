using System.Reflection;

namespace MicroservicesCafe.Products.Infrastructure;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
