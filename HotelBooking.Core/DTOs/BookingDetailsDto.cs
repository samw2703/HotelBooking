namespace HotelBooking.Core.DTOs;

public class BookingDetailsDto
{
    public string BookingReference { get; set; } = string.Empty;
    public string HotelName { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public int Guests { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}