using HotelBooking.Core.DTOs;

namespace HotelBooking.Core.Services.Bookings;

public interface IBookingService
{
    Task<BookingResponseDto> BookRoomAsync(BookingRequestDto request);
    Task<RoomDto[]> FindAvailable(DateOnly startDate, DateOnly endDate, int numberOfGuests);
    Task<BookingDetailsDto?> GetBookingDetails(string reference);
}