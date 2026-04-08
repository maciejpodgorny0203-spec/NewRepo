using HotelReservationApp.Helpers;
using HotelReservationApp.Models;

namespace HotelReservationApp.Services;

public class CsvFileService
{
    public List<Room> LoadRooms(string path)
    {
        var rooms = new List<Room>();

        if (!File.Exists(path))
            return rooms;

        var lines = File.ReadAllLines(path).Skip(1);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = line.Split(',');

            if (parts.Length < 3)
                continue;

            rooms.Add(new Room
            {
                Id = int.Parse(parts[0]),
                Name = parts[1],
                Capacity = int.Parse(parts[2])
            });
        }

        return rooms;
    }

    public List<Reservation> LoadReservations(string path)
    {
        var reservations = new List<Reservation>();

        if (!File.Exists(path))
            return reservations;

        var lines = File.ReadAllLines(path).Skip(1);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = line.Split(',');

            if (parts.Length < 6)
                continue;

            if (!DateHelper.TryParseDate(parts[2], out DateTime dateFrom))
                continue;

            if (!DateHelper.TryParseDate(parts[3], out DateTime dateTo))
                continue;

            reservations.Add(new Reservation
            {
                Id = int.Parse(parts[0]),
                RoomId = int.Parse(parts[1]),
                DateFrom = dateFrom,
                DateTo = dateTo,
                ReservedBy = parts[4],
                NumberOfGuests = int.Parse(parts[5])
            });
        }

        return reservations;
    }

    public void SaveReservations(string path, List<Reservation> reservations)
    {
        var lines = new List<string>
        {
            "Id,RoomId,DateFrom,DateTo,ReservedBy,NumberOfGuests"
        };

        lines.AddRange(reservations.Select(r =>
            $"{r.Id},{r.RoomId},{DateHelper.ToCsvDate(r.DateFrom)},{DateHelper.ToCsvDate(r.DateTo)},{r.ReservedBy},{r.NumberOfGuests}"
        ));

        File.WriteAllLines(path, lines);
    }

    public void SaveReport(string path, List<Reservation> reservations)
    {
        var lines = new List<string>
        {
            "Id,RoomId,DateFrom,DateTo,ReservedBy,NumberOfGuests"
        };

        lines.AddRange(reservations.Select(r =>
            $"{r.Id},{r.RoomId},{DateHelper.ToCsvDate(r.DateFrom)},{DateHelper.ToCsvDate(r.DateTo)},{r.ReservedBy},{r.NumberOfGuests}"
        ));

        File.WriteAllLines(path, lines);
    }
}
