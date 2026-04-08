using System.Globalization;

namespace HotelReservationApp.Helpers;

public static class DateHelper
{
    private const string DateFormat = "yyyy-MM-dd";

    public static bool TryParseDate(string input, out DateTime date)
    {
        return DateTime.TryParseExact(
            input,
            DateFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out date);
    }

    public static string ToCsvDate(DateTime date)
    {
        return date.ToString(DateFormat);
    }
}
