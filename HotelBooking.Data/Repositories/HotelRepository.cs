using HotelBooking.Core.Entities;
using HotelBooking.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data.Repositories;

internal class HotelRepository : IHotelRepository
{
    private readonly ApplicationDbContext _context;

    public HotelRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Hotel?> FindByName(string name) => _context.Hotels
        .Include(x => x.Rooms).ThenInclude(x => x.Bookings)
        .SingleOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());

    public Task<Hotel[]> GetAll() => _context.Hotels
        .Include(x => x.Rooms).ThenInclude(x => x.Bookings)
        .ToArrayAsync();

    public Task<Room?> FindRoomById(int roomId) => _context.Hotels
        .Include(x => x.Rooms).ThenInclude(x => x.Bookings)
        .SelectMany(x => x.Rooms)
        .SingleOrDefaultAsync(x => x.RoomId == roomId);

    public async Task Save(Hotel hotel)
    {
        if (hotel.HotelId == 0)
            await _context.AddAsync(hotel);

        await _context.SaveChangesAsync();
    }

    public async Task Delete(Hotel hotel)
    {
        _context.Remove(hotel);
        await _context.SaveChangesAsync();
    }
}