using HotelBooking.Core.DTOs;
using HotelBooking.Core.Entities;
using HotelBooking.Core.Exceptions;
using HotelBooking.Core.Repositories;

namespace HotelBooking.Core.Services.Bookings;

internal class BookingService(IHotelRepository hotelRepository, IBookingRepository bookingRepository) : IBookingService
{
    public async Task<BookingResponseDto> BookRoomAsync(BookingRequestDto request)
    {
        var room = await hotelRepository.FindRoomById(request.RoomId);

        var existingBooking = await bookingRepository.GetByIdempotencyKey(request.IdempotencyKey);

        if (existingBooking != null)
            MapToBookingResponse(existingBooking);

        if (room == null)
            throw new RoomNotFoundException();

        if (!IsAvailable(room, request.StartDate, request.EndDate, request.NumberOfGuests))
            throw new RoomNotAvailableException();

        var booking = new Booking
        {
            RoomId = request.RoomId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            NumberOfGuests = request.NumberOfGuests,
            BookingReference = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
            IdempotencyKey = request.IdempotencyKey
        };

        await bookingRepository.Save(booking);

        return MapToBookingResponse(booking);
    }

    public async Task<RoomDto[]> FindAvailable(DateOnly startDate, DateOnly endDate, int numberOfGuests)
    {
        var hotels = await hotelRepository.GetAll();

        return hotels
            .SelectMany(hotel => hotel.Rooms)
            .Where(room => IsAvailable(room, startDate, endDate, numberOfGuests))
            .Select(room => new RoomDto
            {
                RoomId = room.RoomId,
                HotelId = room.HotelId,
                RoomType = room.RoomType.ToString(),
                Capacity = room.Capacity
            })
            .ToArray();
    }

    public async Task<BookingDetailsDto?> GetBookingDetails(string reference)
    {
        var booking = await bookingRepository.GetByReference(reference);

        return booking == null
            ? null
            : new BookingDetailsDto
            {
                BookingReference = booking.BookingReference,
                HotelName = booking.Room.Hotel.Name,
                RoomType = booking.Room.RoomType.ToString(),
                Guests = booking.NumberOfGuests,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate
            };
    }

    private static bool IsAvailable(Room room, DateOnly startDate, DateOnly endDate, int numberOfGuests)
        => room.Capacity >= numberOfGuests && !room.Bookings.Any(x => Clashes(x, startDate, endDate));

    private static bool Clashes(Booking booking, DateOnly startDate, DateOnly endDate)
        => booking.StartDate < endDate && startDate < booking.EndDate;

    private static BookingResponseDto MapToBookingResponse(Booking booking) => new BookingResponseDto
    {
        BookingId = booking.BookingId,
        RoomId = booking.RoomId,
        StartDate = booking.StartDate,
        EndDate = booking.EndDate,
        NumberOfGuests = booking.NumberOfGuests,
        BookingReference = booking.BookingReference,
        IdempotencyKey = booking.IdempotencyKey,
    };
}