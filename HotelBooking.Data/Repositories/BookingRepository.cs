using HotelBooking.Core.Entities;
using HotelBooking.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data.Repositories;

internal class BookingRepository(ApplicationDbContext context) : IBookingRepository
{
    public async Task Save(Booking booking)
    {
        if (booking.BookingId == 0)
            await context.AddAsync(booking);

        await context.SaveChangesAsync();
    }

    public Task<Booking?> GetByIdempotencyKey(string key)
        => context.Bookings.SingleOrDefaultAsync(b => b.IdempotencyKey == key);

    public Task<Booking?> GetByReference(string bookingReference) => context.Bookings
            .Include(b => b.Room).ThenInclude(r => r.Hotel)
            .SingleOrDefaultAsync(b => b.BookingReference == bookingReference);
}