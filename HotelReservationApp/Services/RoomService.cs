using HotelReservationApp.Models;

namespace HotelReservationApp.Services;

public class RoomService
{
    private readonly CsvFileService _csvFileService;
    private readonly string _roomsPath;
    private readonly string _reservationsPath;

    public RoomService(CsvFileService csvFileService, string roomsPath, string reservationsPath)
    {
        _csvFileService = csvFileService;
        _roomsPath = roomsPath;
        _reservationsPath = reservationsPath;
    }

    public List<Room> GetAllRooms()
    {
        return _csvFileService.LoadRooms(_roomsPath);
    }

    public Room? GetRoomById(int roomId)
    {
        return GetAllRooms().FirstOrDefault(r => r.Id == roomId);
    }

    public List<Room> GetAvailableRooms(DateTime dateFrom, DateTime dateTo)
    {
        var rooms = _csvFileService.LoadRooms(_roomsPath);
        var reservations = _csvFileService.LoadReservations(_reservationsPath);

        return rooms.Where(room =>
            !reservations.Any(r =>
                r.RoomId == room.Id &&
                dateFrom < r.DateTo &&
                dateTo > r.DateFrom))
            .ToList();
    }
}
