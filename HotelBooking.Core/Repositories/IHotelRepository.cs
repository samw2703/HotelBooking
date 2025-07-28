using HotelBooking.Core.Entities;

namespace HotelBooking.Core.Repositories;

public interface IHotelRepository 
{
    Task<Hotel?> FindByNameAsync(string name);
}