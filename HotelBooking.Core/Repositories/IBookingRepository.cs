using HotelBooking.Core.Entities;

namespace HotelBooking.Core.Repositories;

public interface IBookingRepository 
{
    Task Save(Booking booking);
    Task<Booking?> GetByIdempotencyKey(string key);
    Task<Booking?> GetByReference(string bookingReference);

}