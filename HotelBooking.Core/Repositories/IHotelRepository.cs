using HotelBooking.Core.Entities;

namespace HotelBooking.Core.Repositories;

public interface IHotelRepository 
{
    Task<Hotel?> FindByName(string name);
    Task<Hotel[]> GetAll();
    Task<Room?> FindRoomById(int roomId);
    Task Save(Hotel hotel);
    Task Delete(Hotel hotel);
}
