using System.Reflection;

namespace ColorMicroservice.Shared.Initialization;

public static class AssemblyLoader
{
    public static HashSet<Assembly> LoadAssemblies(string assemblyName)
    {
        var assemblies = LoadNestedAssemblies(Assembly.GetEntryAssembly(), assemblyName)
            .SelectMany(assembly =>
            {
                var nestedAssemblies = LoadNestedAssemblies(assembly, assemblyName);

                return nestedAssemblies.Concat(
                [
                    assembly
                ]);
            })
            .ToHashSet();

        return assemblies;
    }

    private static IEnumerable<Assembly> LoadNestedAssemblies(Assembly? assembly, string assemblyName)
        => assembly is null
            ? []
            : assembly.GetReferencedAssemblies()
                .Where(a => a.Name?.StartsWith($"{assemblyName}.") ?? false)
                .Select(Assembly.Load);
}
