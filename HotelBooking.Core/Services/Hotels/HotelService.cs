using HotelBooking.Core.DTOs;

namespace HotelBooking.Core.Services.Hotels;

internal class HotelService : IHotelService
{
    public Task<HotelDto?> FindByNameAsync(string name)
    {
        throw new NotImplementedException();
    }
}