using HotelReservationApp.Helpers;

namespace HotelReservationApp.Helpers;

public static class ConsoleHelper
{
    public static int ReadInt(string message)
    {
        while (true)
        {
            Console.Write(message);
            var input = Console.ReadLine();

            if (int.TryParse(input, out int value))
                return value;

            Console.WriteLine("Nieprawidłowa liczba. Spróbuj ponownie.");
        }
    }

    public static string ReadRequiredString(string message)
    {
        while (true)
        {
            Console.Write(message);
            var input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
                return input.Trim();

            Console.WriteLine("Pole nie może być puste.");
        }
    }

    public static DateTime ReadDate(string message)
    {
        while (true)
        {
            Console.Write(message);
            var input = Console.ReadLine();

            if (input != null && DateHelper.TryParseDate(input, out DateTime date))
                return date;

            Console.WriteLine("Nieprawidłowy format daty. Użyj yyyy-MM-dd.");
        }
    }

    public static void ShowHeader(string title)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 40));
        Console.WriteLine(title);
        Console.WriteLine(new string('=', 40));
    }
}
