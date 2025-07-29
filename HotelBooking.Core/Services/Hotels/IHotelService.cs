using HotelBooking.Core.DTOs;

namespace HotelBooking.Core.Services.Hotels;

public interface IHotelService 
{
    Task<HotelDto?> FindByName(string name);
    Task<HotelDto[]> GetAll();
}
