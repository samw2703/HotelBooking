namespace HotelBooking.Core.Entities;    

public class Hotel
{
    public int HotelId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Room> Rooms { get; set; } = [];
}
