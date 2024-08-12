using System.Reflection;

namespace MicroservicesCafe.Product.Persistance;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
