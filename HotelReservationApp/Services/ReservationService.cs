using HotelReservationApp.Models;

namespace HotelReservationApp.Services;

public class ReservationService
{
    private readonly CsvFileService _csvFileService;
    private readonly string _roomsPath;
    private readonly string _reservationsPath;

    public ReservationService(CsvFileService csvFileService, string roomsPath, string reservationsPath)
    {
        _csvFileService = csvFileService;
        _roomsPath = roomsPath;
        _reservationsPath = reservationsPath;
    }

    public List<Reservation> GetAllReservations()
    {
        return _csvFileService.LoadReservations(_reservationsPath)
            .OrderBy(r => r.DateFrom)
            .ToList();
    }

    public string AddReservation(Reservation newReservation)
    {
        var rooms = _csvFileService.LoadRooms(_roomsPath);
        var reservations = _csvFileService.LoadReservations(_reservationsPath);

        var room = rooms.FirstOrDefault(r => r.Id == newReservation.RoomId);
        if (room == null)
            return "Pokój o podanym ID nie istnieje.";

        if (newReservation.DateFrom >= newReservation.DateTo)
            return "Data początkowa musi być wcześniejsza niż data końcowa.";

        if (newReservation.NumberOfGuests > room.Capacity)
            return "Liczba gości przekracza pojemność pokoju.";

        bool hasConflict = reservations.Any(r =>
            r.RoomId == newReservation.RoomId &&
            newReservation.DateFrom < r.DateTo &&
            newReservation.DateTo > r.DateFrom);

        if (hasConflict)
            return "Pokój jest już zajęty w podanym terminie.";

        newReservation.Id = reservations.Any() ? reservations.Max(r => r.Id) + 1 : 1;
        reservations.Add(newReservation);

        _csvFileService.SaveReservations(_reservationsPath, reservations);

        return "Rezerwacja została dodana poprawnie.";
    }

    public string DeleteReservation(int reservationId)
    {
        var reservations = _csvFileService.LoadReservations(_reservationsPath);
        var reservation = reservations.FirstOrDefault(r => r.Id == reservationId);

        if (reservation == null)
            return "Nie znaleziono rezerwacji o podanym ID.";

        reservations.Remove(reservation);
        _csvFileService.SaveReservations(_reservationsPath, reservations);

        return "Rezerwacja została usunięta.";
    }

    public string UpdateReservation(Reservation updatedReservation)
    {
        var rooms = _csvFileService.LoadRooms(_roomsPath);
        var reservations = _csvFileService.LoadReservations(_reservationsPath);

        var existingReservation = reservations.FirstOrDefault(r => r.Id == updatedReservation.Id);
        if (existingReservation == null)
            return "Nie znaleziono rezerwacji o podanym ID.";

        var room = rooms.FirstOrDefault(r => r.Id == updatedReservation.RoomId);
        if (room == null)
            return "Pokój o podanym ID nie istnieje.";

        if (updatedReservation.DateFrom >= updatedReservation.DateTo)
            return "Data początkowa musi być wcześniejsza niż data końcowa.";

        if (updatedReservation.NumberOfGuests > room.Capacity)
            return "Liczba gości przekracza pojemność pokoju.";

        bool hasConflict = reservations.Any(r =>
            r.Id != updatedReservation.Id &&
            r.RoomId == updatedReservation.RoomId &&
            updatedReservation.DateFrom < r.DateTo &&
            updatedReservation.DateTo > r.DateFrom);

        if (hasConflict)
            return "Pokój jest już zajęty w podanym terminie.";

        existingReservation.RoomId = updatedReservation.RoomId;
        existingReservation.DateFrom = updatedReservation.DateFrom;
        existingReservation.DateTo = updatedReservation.DateTo;
        existingReservation.ReservedBy = updatedReservation.ReservedBy;
        existingReservation.NumberOfGuests = updatedReservation.NumberOfGuests;

        _csvFileService.SaveReservations(_reservationsPath, reservations);

        return "Rezerwacja została zaktualizowana.";
    }
}
