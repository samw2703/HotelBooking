using HotelBooking.Core.DTOs;

namespace HotelBooking.Core.Services.Hotels;

public interface IHotelService 
{
    Task<HotelDto?> FindByNameAsync(string name);
}
