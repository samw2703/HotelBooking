namespace HotelBooking.Core.Entities;

public class Room
{
    public int RoomId { get; set; }
    public int HotelId { get; set; }
    public Hotel Hotel { get; set; }
    public RoomType RoomType { get; set; }
    public int Capacity { get; set; }
    public ICollection<Booking> Bookings { get; set; } = [];
}

public enum RoomType { Single, Double, Deluxe }
