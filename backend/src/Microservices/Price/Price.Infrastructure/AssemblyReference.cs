using System.Reflection;

namespace Price.Infrastructure;
public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}

