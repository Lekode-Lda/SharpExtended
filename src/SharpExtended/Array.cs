namespace SharpExtended;

public static class ArrayExtension {
    /// <summary>
    /// Removes a item from the array at a specific index
    /// </summary>
    /// <param name="indicesArray">The array to remove the item from</param>
    /// <param name="removeAt">Index of the item to remove</param>
    /// <typeparam name="T">Type of the array. Inferred from the indicesArray param</typeparam>
    /// <returns>A new array</returns>
    public static T[] RemoveIndices<T>(this T[] indicesArray, int removeAt) {
        var newIndicesArray = new T[indicesArray.Length - 1];

        var i = 0;
        var j = 0;

        while (i < indicesArray.Length) {
            if (i != removeAt) {
                newIndicesArray[j] = indicesArray[i];
                j++; 
            }

            i++;
        }

        return newIndicesArray;
    }

    /// <summary>
    /// Adds a new element to a array
    /// </summary>
    /// <param name="array">The array to add the element to</param>
    /// <param name="element">Element to add to the array</param>
    /// <typeparam name="T">Type of the array. Inferred from the parameter array</typeparam>
    /// <returns>A new array with the new item</returns>
    public static T[] AddElementToArray<T>(this T[] array, T element) {
        var newArray = new T[array.Length + 1];
        int i;
        for (i = 0; i < array.Length; i++)
            newArray[i] = array[i];

        newArray[i] = element;
        return newArray;
    }

    /// <summary>
    /// Truncates each string in a array
    /// </summary>
    /// <param name="arr">Array of strings to truncate</param>
    /// <param name="maxLength">Maximum length of each string</param>
    /// <returns>Returns a array with the truncated strings</returns>
    public static string[] TruncateStrings(this string[] arr, int maxLength) {
        for (var i = 0; i < arr.Length; i++)
            arr[i] = arr[i].Truncate(maxLength);
        return arr;
    }

    /// <summary>
    /// Converts a string array to a int array
    /// </summary>
    /// <param name="arr">Array of strings</param>
    /// <returns>Array of int values</returns>
    public static int[] ToIntArray(this string[] arr) {
        var tmpArr = new int[arr.Length];

        for (var i = 0; i < arr.Length; i++)
            tmpArr[i] = int.Parse(arr[i]);
        return tmpArr;
    }
    
    /// <summary>
    /// Trims all strings in a array
    /// </summary>
    /// <param name="arr">Array of strings</param>
    /// <returns>A new array with the trimmed strings</returns>
    public static string[] TrimStrings(this string[] arr) {
        for (var i = 0; i < arr.Length; i++)
            arr[i] = arr[i].Trim();
        return arr;
    }

    /// <summary>
    /// Converts an array of Enums to string
    /// </summary>
    /// <param name="enumerator">The array of enums</param>
    /// <typeparam name="T">Type of the num</typeparam>
    /// <returns>An array with the strings corresponding to each enum value in the array</returns>
    public static string?[] ToStringArray<T>(this T[] enumerator) where T : Enum => 
        enumerator.Select(elm => elm.GetDescription()).ToArray();
    
    /// <summary>
    /// Converts a array of strings to a list of Enums to the corresponding string
    /// Elements can't parsed will be ignored
    /// </summary>
    /// <param name="arr">Array of strings to parse</param>
    /// <typeparam name="T">Enum to parse to</typeparam>
    /// <returns></returns>
    public static List<T> ToList<T>(IEnumerable<string> arr) where T : Enum {
        var list = new List<T>();
        foreach(var elm in arr) {
            try {
                list.Add(elm.ParseEnum<T>());
                // ReSharper disable once EmptyGeneralCatchClause
            } catch (Exception) { }
        }

        return list;
    }
    
}