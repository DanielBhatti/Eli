using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Common.Linq;

public static class LinqExtensions
{
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection) => new(collection);
}
