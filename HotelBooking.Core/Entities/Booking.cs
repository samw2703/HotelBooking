namespace HotelBooking.Core.Entities;

public class Booking
{
    public int BookingId { get; set; }
    public int RoomId { get; set; }
    public Room Room { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int NumberOfGuests { get; set; }
    public string BookingReference { get; set; } = string.Empty;
    public string IdempotencyKey { get; set; } = string.Empty;
}