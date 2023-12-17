namespace SharpExtended;

public static class List {

    /// <summary>
    /// Makes all strings in a IEnumerable object lowercase 
    /// </summary>
    /// <param name="list">IEnumerable object of strings to lowercase</param>
    /// <returns>A list with the lowercased strings</returns>
    public static List<string> StringsToLowerCase(this IEnumerable<string> list) =>
        list.Select(itm => itm.ToLower()).ToList();
    
    /// <summary>
    /// Checks if the two lists are equals (discarding the order)
    /// </summary>
    /// <param name="list1"></param>
    /// <param name="list2"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool ScrambledEquals<T>(IEnumerable<T> list1, IEnumerable<T> list2) where T : notnull {
        var cnt = new Dictionary<T, int>();
        foreach (var s in list1) {
            if (!cnt.TryAdd(s, 1))
                cnt[s]++;
        }
        foreach (var s in list2) {
            if (cnt.TryGetValue(s, out var value)) {
                cnt[s] = --value;
            } else {
                return false;
            }
        }
        return cnt.Values.All(c => c == 0);
    }

    /// <summary>
    /// Trims all strings in a list
    /// </summary>
    /// <param name="list"></param>
    /// <returns>List with all trimmed strings</returns>
    public static List<string> TrimStrings(this IEnumerable<string> list) =>
        list.Select(elm => elm.Trim()).ToList();

}