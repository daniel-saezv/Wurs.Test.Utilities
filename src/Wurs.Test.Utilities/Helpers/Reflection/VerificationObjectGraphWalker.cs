using System.Reflection;

namespace Wurs.Test.Utilities.Helpers.Reflection;

internal static class VerificationObjectGraphWalker
{
    internal static void Walk(
        object root,
        Func<object, Assembly, bool> shouldRecurse,
        Action<object> visit)
    {
        ArgumentNullException.ThrowIfNull(root);
        ArgumentNullException.ThrowIfNull(shouldRecurse);
        ArgumentNullException.ThrowIfNull(visit);

        var seen = new HashSet<object>(ReferenceEqualityComparer.Instance);
        WalkRecursive(root, root.GetType().Assembly, seen, shouldRecurse, visit);
    }

    internal static bool ShouldRecurseInTestObjectGraph(object value, Assembly testAssembly)
    {
        var type = value.GetType();

        if (type == typeof(string) || type.IsPrimitive || type.IsEnum)
        {
            return false;
        }

        return type.Assembly == testAssembly || (type.Namespace is not null && type.Namespace.StartsWith("Wurs.", StringComparison.Ordinal));
    }

    private static void WalkRecursive(
        object current,
        Assembly testAssembly,
        HashSet<object> seen,
        Func<object, Assembly, bool> shouldRecurse,
        Action<object> visit)
    {
        if (!seen.Add(current))
        {
            return;
        }

        visit(current);

        foreach (var value in GetMemberValues(current))
        {
            if (value is null)
            {
                continue;
            }

            visit(value);

            if (shouldRecurse(value, testAssembly))
            {
                WalkRecursive(value, testAssembly, seen, shouldRecurse, visit);
            }
        }
    }

    private static IEnumerable<object?> GetMemberValues(object instance)
    {
        var currentType = instance.GetType();

        while (currentType is not null)
        {
            foreach (var field in currentType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
            {
                yield return field.GetValue(instance);
            }

            foreach (var property in currentType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
            {
                if (property.GetIndexParameters().Length > 0 || property.GetMethod is null)
                {
                    continue;
                }

                yield return property.GetValue(instance);
            }

            currentType = currentType.BaseType;
        }
    }
}
