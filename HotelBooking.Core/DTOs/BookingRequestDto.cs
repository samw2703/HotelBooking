namespace HotelBooking.Core.DTOs;

public class BookingRequestDto 
{
    public int RoomId { get; set; }
    public int NumberOfGuests { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string IdempotencyKey { get; set; } = string.Empty;
}
