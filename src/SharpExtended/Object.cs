using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SharpExtended;

public static class ObjectExtensions {
    
    /// <summary>
    /// Copies a object onto another
    /// </summary>
    /// <param name="from">Object to copy from</param>
    /// <param name="to">Object to copy to</param>
    /// <param name="ignoreKey">Ignores the parameter with the attribute [Key] when true</param>
    /// <typeparam name="T">Type of the object</typeparam>
    public static void CopyTo<T>(this T from, T to, bool ignoreKey = true) {
        var t     = typeof (T);
        var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var p in props) {
            if(Attribute.IsDefined(p, typeof(KeyAttribute)) && ignoreKey) continue;
            if (!p.CanRead || !p.CanWrite) continue;

            var val        = p.GetGetMethod()?.Invoke(from, default);
            var defaultVal = p.PropertyType.IsValueType ? Activator.CreateInstance(p.PropertyType) : default;
            if (val != null && !val.Equals(defaultVal))
                p.GetSetMethod()?.Invoke(to, new[] {val});
        }
    }
    
}