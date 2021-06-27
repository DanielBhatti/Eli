using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Reflection
{
    public static class ClassManager
    {
        public static List<Type> GetDerivedClasses(Type type)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(type.GetType()) && !t.IsAbstract)
                .ToList();
        }
    }
}
