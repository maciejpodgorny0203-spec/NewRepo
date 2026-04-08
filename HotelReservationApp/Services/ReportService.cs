using HotelReservationApp.Helpers;
using HotelReservationApp.Models;

namespace HotelReservationApp.Services;

public class ReportService
{
    private readonly CsvFileService _csvFileService;
    private readonly string _reservationsPath;

    public ReportService(CsvFileService csvFileService, string reservationsPath)
    {
        _csvFileService = csvFileService;
        _reservationsPath = reservationsPath;
    }

    public string GenerateRoomReport(int roomId, DateTime dateFrom, DateTime dateTo)
    {
        var reservations = _csvFileService.LoadReservations(_reservationsPath);

        var filteredReservations = reservations
            .Where(r => r.RoomId == roomId &&
                        r.DateFrom < dateTo &&
                        r.DateTo > dateFrom)
            .OrderBy(r => r.DateFrom)
            .ToList();

        var fileName = $"report_room_{roomId}_{DateHelper.ToCsvDate(dateFrom)}_{DateHelper.ToCsvDate(dateTo)}.csv";
        var path = Path.Combine("Data", fileName);

        _csvFileService.SaveReport(path, filteredReservations);

        return path;
    }
}
