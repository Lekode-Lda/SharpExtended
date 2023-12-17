// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable CollectionNeverQueried.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace SharpExtended;

public class Result {

    public bool         Success    { get; }
    public string       Error      { get; private set; }
    public List<string> StackTrace { get; } = [];
    public bool         IsFailure  => !Success;

    protected Result(bool success, string error) {
        switch (success) {
            case true when error != string.Empty:
                throw new InvalidOperationException();
            case false when error == string.Empty:
                throw new InvalidOperationException();
        }

        Success    = success;
        Error      = error;
    }

    /// <summary>
    /// Returns a fail without a value
    /// </summary>
    /// <param name="message">Error message</param>
    /// <returns>Result instance</returns>
    public static Result Fail(string message) => new(false, message);
    
    /// <summary>
    /// Returns a fail with a value of type T
    /// </summary>
    /// <param name="message">Error message</param>
    /// <typeparam name="T">Data type to compute the default value</typeparam>
    /// <returns>Result instance</returns>
    public static Result<T?> Fail<T>(string message) => new(default, false, message);
    
    /// <summary>
    /// Returns a ok without value
    /// </summary>
    /// <returns>Result instance</returns>
    public static Result Ok() => new(true, string.Empty);
    
    /// <summary>
    /// Returns a ok with a value of type T
    /// </summary>
    /// <param name="value">Value to return</param>
    /// <typeparam name="T">Data type</typeparam>
    /// <returns></returns>
    public static Result<T?> Ok<T>(T value) => new(value, true, string.Empty);

    /// <summary>
    /// Updates the current error message pushing the existing one to the top of the <see cref="StackTrace"/>
    /// </summary>
    /// <param name="message">Error message to update</param>
    public void AddFail(string message) {
        StackTrace.Insert(0, Error);
        Error = message;
    } 
}

public class Result<T>: Result {
    public T Value { get; set; }

    protected internal Result(T value, bool success, string error) : base(success, error) => Value = value;
}