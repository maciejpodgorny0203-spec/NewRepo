namespace HotelReservationApp.Models;

public class Reservation
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public string ReservedBy { get; set; } = string.Empty;
    public int NumberOfGuests { get; set; }
}
