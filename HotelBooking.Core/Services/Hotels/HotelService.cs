using HotelBooking.Core.DTOs;
using HotelBooking.Core.Entities;
using HotelBooking.Core.Repositories;

namespace HotelBooking.Core.Services.Hotels;

internal class HotelService(IHotelRepository hotelRepository) : IHotelService
{
    public async Task<HotelDto?> FindByName(string name)
    {
        var hotel = await hotelRepository.FindByName(name);

        return hotel == null ? null : new HotelDto { HotelId = hotel.HotelId, Name = hotel.Name };
    }

    public async Task<HotelDto[]> GetAll() 
        => (await hotelRepository.GetAll()).Select(Map).ToArray();

    private static HotelDto Map(Hotel hotel) => new HotelDto { HotelId = hotel.HotelId, Name = hotel.Name };
}