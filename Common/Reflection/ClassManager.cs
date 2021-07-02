using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Reflection
{
    public static class ClassManager
    {
        public static List<Type> GetDerivedClasses(Type type)
        {
            return (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                        // alternative: from domainAssembly in domainAssembly.GetExportedTypes()
                    from assemblyType in domainAssembly.GetTypes()
                    //where type.IsAssignableFrom(assemblyType)
                    where assemblyType.IsSubclassOf(type)
                    // alternative: && ! assemblyType.IsAbstract
                    select assemblyType).ToList();
        }
    }
}
