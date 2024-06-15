using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Eli.Reflection;

public static class ReflectionMetadata
{
    public static IList<Type> GetDerivedClasses(Type type)
    {
        return (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                    // alternative: from domainAssembly in domainAssembly.GetExportedTypes()
                from assemblyType in domainAssembly.GetTypes()
                    //where type.IsAssignableFrom(assemblyType)
                where assemblyType.IsSubclassOf(type)
                // alternative: && ! assemblyType.IsAbstract
                select assemblyType).ToList();
    }

    public static bool HasParameterlessConstructor(this Type type, bool includeNonPublic = false)
    {
        if(type.IsValueType && Nullable.GetUnderlyingType(type) == null) return true;

        var constructor = includeNonPublic
            ? type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null)
            : type.GetConstructor(Type.EmptyTypes);
        return constructor != null;
    }

    public static object ConvertEnumerable(IEnumerable<object> value, Type type)
    {
        var containedType = type.GenericTypeArguments.First();
        return value.Select(item => Convert.ChangeType(item, containedType)).ToList();
    }

    public static T Parse<T>(string value) => (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(value)!;
}
