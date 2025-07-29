namespace HotelBooking.Core.DTOs;

public class RoomDto
{
    public int RoomId { get; set; }
    public int HotelId { get; set; }
    public string RoomType { get; set; }
    public int Capacity { get; set; }
}
