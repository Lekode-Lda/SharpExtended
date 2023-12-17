// ReSharper disable InconsistentNaming
namespace SharpExtended;

public static class IEnumerable {
    
    /// <summary>
    /// Indexed ForEach. Does not support break condition
    /// </summary>
    /// <param name="ie">IEnumerable to iterate through</param>
    /// <param name="action">Action to perform on the foreach</param>
    /// <typeparam name="T">Type of the IEnumerable</typeparam>
    public static void Each<T>(this IEnumerable<T> ie, Action<T, int> action) {
        var i = 0;
        foreach (var e in ie) action(e, i++);
    }
    
}