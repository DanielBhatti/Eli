using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Reflection
{
    public static class TypeManager
    {
        public static object ConvertEnumerable(IEnumerable<object> value, Type type)
        {
            Type containedType = type.GenericTypeArguments.First();
            return value.Select(item => Convert.ChangeType(item, containedType)).ToList();
        }

        public static T Parse<T>(string value) => (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(value);
    }
}
