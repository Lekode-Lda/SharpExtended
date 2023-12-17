using System.Globalization;

namespace SharpExtended;

public static class DateTimeExtensions {
    /// <summary>
    /// Enum for the Accuracy of a DateTime approximation
    /// </summary>
    public enum Accuracy { Year, Quarter, Month, Week, Day, Hour, Minute, Second};

    /// <summary>
    /// Truncates a DateTime to a certain position
    /// </summary>
    /// <param name="inDate">DateTime to be truncated</param>
    /// <param name="accuracy">Accuracy of the truncation. See <see cref="Accuracy"/></param>
    /// <returns>Truncated DateTime</returns>
    /// <exception cref="ArgumentOutOfRangeException">Invalid accuracy</exception>
    public static DateTime TruncateDate(this DateTime inDate, Accuracy accuracy){
        switch (accuracy) {
            case Accuracy.Year:
                return new DateTime(inDate.Year, 1, 1);
            case Accuracy.Quarter:
                var i = inDate.Month % 3;
                return new DateTime(inDate.Year, inDate.Month - i + 1, 1);
            case Accuracy.Month:
                return new DateTime(inDate.Year, inDate.Month, 1);
            case Accuracy.Week:
                return new DateTime(inDate.Year, inDate.Month, inDate.Day).AddDays(-(int)inDate.DayOfWeek);
            case Accuracy.Day:
                return new DateTime(inDate.Year, inDate.Month, inDate.Day);
            case Accuracy.Hour:
                return new DateTime(inDate.Year, inDate.Month, inDate.Day, inDate.Hour, 0, 0);
            case Accuracy.Minute:
                return new DateTime(inDate.Year, inDate.Month, inDate.Day, inDate.Hour, inDate.Minute, 0);
            case Accuracy.Second:
                return new DateTime(inDate.Year, inDate.Month, inDate.Day, inDate.Hour, inDate.Minute, inDate.Second);
            default:
                throw new ArgumentOutOfRangeException(nameof(accuracy));
        }
    }

    /// <summary>
    /// Rounds up a date time
    /// </summary>
    /// <param name="dt">DateTime to round up</param>
    /// <param name="d">From where to round</param>
    /// <returns>Returns the rounded up DateTime</returns>
    public static DateTime RoundUp(this DateTime dt, TimeSpan d) {
        return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
    }

    /// <summary>
    /// Adds a certain amount of weeks to a DateTime
    /// </summary>
    /// <param name="date">DateTime to add weeks to</param>
    /// <param name="value">Number of weeks to add</param>
    /// <returns>DateTime with the added weeks</returns>
    public static DateTime AddWeeks(this DateTime date, int value) => date.AddDays(value * 7);

    /// <summary>
    /// Checks if a certain date is in the correct week
    /// </summary>
    /// <param name="date">DateTime to check</param>
    /// <returns>Boolean indicating if the DateTime is in the current week</returns>
    private static bool IsInWeek(this DateTime date)  {
        var startOfWeek = DateTime.Today.ToUniversalTime().AddDays(
            (int)CultureInfo.InvariantCulture.DateTimeFormat.FirstDayOfWeek -
            (int)DateTime.Today.DayOfWeek);
        var weekRange = Enumerable.Range(0, 7).Select(i => startOfWeek.AddDays(i));
        var last = weekRange.Last();
        return (date >= startOfWeek && date <= last);
    }

    /// <summary>
    /// Checks if date is in current month
    /// </summary>
    /// <param name="date">DateTo check</param>
    /// <returns>Boolean indicating if the DateTime is in the current month</returns>
    private static bool IsInMonth(this DateTime date) {
        var today = DateTime.UtcNow;
        var start = new DateTime(today.Year, today.Month, 1);
        var end = start.AddDays(DateTime.DaysInMonth(today.Year, today.Month));
        return (date >= start && date <= end);
    }

    /// <summary>
    /// Alias for IsInMonth
    /// Doesn't work as extension
    /// </summary>
    /// <param name="date">DateTime time to check</param>
    /// <returns>Boolean indicating if the DateTime is in the current month</returns>
    public static bool InMonth(DateTime date) => date.IsInMonth();
    
    /// <summary>
    /// Alias for IsInWeek
    /// Doesn't work as extension
    /// </summary>
    /// <param name="date">DateTime to check</param>
    /// <returns>Boolean indicating if the DateTime is in the current week</returns>
    public static bool InWeek(DateTime date) => date.IsInWeek();
    
    /// <summary>
    /// Gets the current week
    /// </summary>
    /// <returns>Tuple with the start and end of the week</returns>
    public static (DateTime, DateTime) CurrentWeek() {
        var startOfWeek = DateTime.Today.ToUniversalTime().AddDays(
            (int)CultureInfo.InvariantCulture.DateTimeFormat.FirstDayOfWeek -
            (int)DateTime.Today.DayOfWeek);
        var weekRange = Enumerable.Range(0, 7).Select(i => startOfWeek.AddDays(i));
        var last = weekRange.Last();
        return (startOfWeek.Date, last.Date);
    }

    /// <summary>
    /// Gets the current month
    /// </summary>
    /// <returns>Tuple with the start and end of the month</returns>
    public static (DateTime, DateTime) CurrentMonth() {
        var today = DateTime.UtcNow;
        var start = new DateTime(today.Year, today.Month, 1);
        var end = start.AddDays(DateTime.DaysInMonth(today.Year, today.Month));
        return (start.Date, end.Date);
    }
}