using HotelReservationApp.Helpers;
using HotelReservationApp.Models;
using HotelReservationApp.Services;

string roomsPath = Path.Combine("Data", "rooms.csv");
string reservationsPath = Path.Combine("Data", "reservations.csv");

var csvFileService = new CsvFileService();
var roomService = new RoomService(csvFileService, roomsPath, reservationsPath);
var reservationService = new ReservationService(csvFileService, roomsPath, reservationsPath);
var reportService = new ReportService(csvFileService, reservationsPath);

while (true)
{
    ConsoleHelper.ShowHeader("SYSTEM REZERWACJI HOTELU");

    Console.WriteLine("1. Wyświetl wszystkie pokoje");
    Console.WriteLine("2. Wyświetl wszystkie rezerwacje");
    Console.WriteLine("3. Dodaj rezerwację");
    Console.WriteLine("4. Edytuj rezerwację");
    Console.WriteLine("5. Usuń rezerwację");
    Console.WriteLine("6. Wyświetl wolne pokoje w terminie");
    Console.WriteLine("7. Wygeneruj raport CSV dla pokoju");
    Console.WriteLine("0. Wyjście");

    int choice = ConsoleHelper.ReadInt("Wybierz opcję: ");

    switch (choice)
    {
        case 1:
            ShowRooms();
            break;

        case 2:
            ShowReservations();
            break;

        case 3:
            AddReservation();
            break;

        case 4:
            UpdateReservation();
            break;

        case 5:
            DeleteReservation();
            break;

        case 6:
            ShowAvailableRooms();
            break;

        case 7:
            GenerateReport();
            break;

        case 0:
            return;

        default:
            Console.WriteLine("Nieprawidłowa opcja.");
            break;
    }

    Console.WriteLine();
    Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
    Console.ReadKey();
    Console.Clear();
}

void ShowRooms()
{
    ConsoleHelper.ShowHeader("LISTA POKOI");
    var rooms = roomService.GetAllRooms();

    foreach (var room in rooms)
    {
        Console.WriteLine($"ID: {room.Id}, Nazwa: {room.Name}, Pojemność: {room.Capacity}");
    }
}

void ShowReservations()
{
    ConsoleHelper.ShowHeader("LISTA REZERWACJI");
    var reservations = reservationService.GetAllReservations();

    foreach (var reservation in reservations)
    {
        Console.WriteLine($"ID: {reservation.Id}, Pokój: {reservation.RoomId}, Od: {reservation.DateFrom:yyyy-MM-dd}, Do: {reservation.DateTo:yyyy-MM-dd}, Rezerwujący: {reservation.ReservedBy}, Goście: {reservation.NumberOfGuests}");
    }
}

void AddReservation()
{
    ConsoleHelper.ShowHeader("DODAJ REZERWACJĘ");

    var reservation = new Reservation
    {
        RoomId = ConsoleHelper.ReadInt("Podaj ID pokoju: "),
        DateFrom = ConsoleHelper.ReadDate("Podaj datę od (yyyy-MM-dd): "),
        DateTo = ConsoleHelper.ReadDate("Podaj datę do (yyyy-MM-dd): "),
        ReservedBy = ConsoleHelper.ReadRequiredString("Podaj imię i nazwisko rezerwującego: "),
        NumberOfGuests = ConsoleHelper.ReadInt("Podaj liczbę gości: ")
    };

    var result = reservationService.AddReservation(reservation);
    Console.WriteLine(result);
}

void UpdateReservation()
{
    ConsoleHelper.ShowHeader("EDYTUJ REZERWACJĘ");

    var updatedReservation = new Reservation
    {
        Id = ConsoleHelper.ReadInt("Podaj ID rezerwacji do edycji: "),
        RoomId = ConsoleHelper.ReadInt("Podaj nowe ID pokoju: "),
        DateFrom = ConsoleHelper.ReadDate("Podaj nową datę od (yyyy-MM-dd): "),
        DateTo = ConsoleHelper.ReadDate("Podaj nową datę do (yyyy-MM-dd): "),
        ReservedBy = ConsoleHelper.ReadRequiredString("Podaj nowe imię i nazwisko rezerwującego: "),
        NumberOfGuests = ConsoleHelper.ReadInt("Podaj nową liczbę gości: ")
    };

    var result = reservationService.UpdateReservation(updatedReservation);
    Console.WriteLine(result);
}

void DeleteReservation()
{
    ConsoleHelper.ShowHeader("USUŃ REZERWACJĘ");

    int reservationId = ConsoleHelper.ReadInt("Podaj ID rezerwacji do usunięcia: ");
    var result = reservationService.DeleteReservation(reservationId);

    Console.WriteLine(result);
}

void ShowAvailableRooms()
{
    ConsoleHelper.ShowHeader("WOLNE POKOJE");

    DateTime dateFrom = ConsoleHelper.ReadDate("Podaj datę od (yyyy-MM-dd): ");
    DateTime dateTo = ConsoleHelper.ReadDate("Podaj datę do (yyyy-MM-dd): ");

    var rooms = roomService.GetAvailableRooms(dateFrom, dateTo);

    if (!rooms.Any())
    {
        Console.WriteLine("Brak wolnych pokoi w podanym terminie.");
        return;
    }

    foreach (var room in rooms)
    {
        Console.WriteLine($"ID: {room.Id}, Nazwa: {room.Name}, Pojemność: {room.Capacity}");
    }
}

void GenerateReport()
{
    ConsoleHelper.ShowHeader("GENEROWANIE RAPORTU");

    int roomId = ConsoleHelper.ReadInt("Podaj ID pokoju: ");
    DateTime dateFrom = ConsoleHelper.ReadDate("Podaj datę od (yyyy-MM-dd): ");
    DateTime dateTo = ConsoleHelper.ReadDate("Podaj datę do (yyyy-MM-dd): ");

    var reportPath = reportService.GenerateRoomReport(roomId, dateFrom, dateTo);
    Console.WriteLine($"Raport zapisano do pliku: {reportPath}");
}
