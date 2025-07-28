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

    public Task<Hotel?> FindByNameAsync(string name)
        => _context.Hotels.SingleOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());    
}
