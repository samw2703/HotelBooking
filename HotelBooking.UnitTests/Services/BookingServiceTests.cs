using HotelBooking.Core.Entities;
using HotelBooking.Core.Repositories;
using HotelBooking.Core.Services.Bookings;
using Moq;

namespace HotelBooking.UnitTests.BookingServiceTests;

public class bookingServiceTests
{
    [Fact]
    public async Task FindAvailableRooms_ShouldReturnRoomsWithEnoughCapacity()
    {
        var mockHotelRepository = new Mock<IHotelRepository>();
        var bookingService = new BookingService(mockHotelRepository.Object, new Mock<IBookingRepository>().Object);

        var hotels = new[]
        {
            new Hotel
            {
                HotelId = 1,
                Name = "Hotel A",
                Rooms = new List<Room>
                {
                    new Room
                    {
                        RoomId = 1,
                        HotelId = 1,
                        Capacity = 2,
                        RoomType = RoomType.Double
                    },
                    new Room
                    {
                        RoomId = 2,
                        HotelId = 1,
                        Capacity = 1,
                        RoomType = RoomType.Single
                    }
                }
            }
        };

        mockHotelRepository.Setup(r => r.GetAll()).ReturnsAsync(hotels);

        var rooms = await bookingService.FindAvailable(DateOnly.Parse("2024-07-09"), DateOnly.Parse("2024-07-10"), 2);

        Assert.Single(rooms);
        Assert.Equal(1, rooms[0].RoomId);
    }

    [Fact]
    public async Task FindAvailableRooms_ShouldExcludeRoomsWithClashingBookings()
    {
        var mockHotelRepository = new Mock<IHotelRepository>();
        var bookingService = new BookingService(mockHotelRepository.Object, new Mock<IBookingRepository>().Object);

        var hotels = new[]
        {
            new Hotel
            {
                HotelId = 1,
                Name = "Hotel A",
                Rooms = new List<Room>
                {
                    new Room
                    {
                        RoomId = 1,
                        HotelId = 1,
                        Capacity = 1,
                        RoomType = RoomType.Single,
                        Bookings = new List<Booking>
                        {
                            new Booking
                            {
                                StartDate = DateOnly.Parse("2024-07-09"),
                                EndDate = DateOnly.Parse("2024-07-12")
                            }
                        }
                    },
                    new Room
                    {
                        RoomId = 2,
                        HotelId = 1,
                        Capacity = 1,
                        RoomType = RoomType.Single,
                        Bookings = new List<Booking>
                        {
                            new Booking
                            {
                                StartDate = DateOnly.Parse("2024-07-09"),
                                EndDate = DateOnly.Parse("2024-07-10")
                            }
                        }
                    },
                    new Room
                    {
                        RoomId = 3,
                        HotelId = 1,
                        Capacity = 1,
                        RoomType = RoomType.Single,
                        Bookings = new List<Booking>
                        {
                            new Booking
                            {
                                StartDate = DateOnly.Parse("2024-07-12"),
                                EndDate = DateOnly.Parse("2024-07-16")
                            }
                        }
                    },
                }
            }
        };

        mockHotelRepository.Setup(r => r.GetAll()).ReturnsAsync(hotels);

        var rooms = await bookingService.FindAvailable(DateOnly.Parse("2024-07-10"), DateOnly.Parse("2024-07-12"), 1);

        Assert.Contains(rooms, room => room.RoomId == 2);
        Assert.Contains(rooms, room => room.RoomId == 3);
        Assert.DoesNotContain(rooms, room => room.RoomId == 1);
    }

    [Fact]
    public async Task FindAvailableRooms_ShouldReturnRoomsFromMultipleHotels()
    {
        var mockHotelRepository = new Mock<IHotelRepository>();
        var bookingService = new BookingService(mockHotelRepository.Object, new Mock<IBookingRepository>().Object);

        var hotels = new[]
        {
            new Hotel
            {
                HotelId = 1,
                Name = "Hotel A",
                Rooms = new List<Room>
                {
                    new Room
                    {
                        RoomId = 1,
                        HotelId = 1,
                        Capacity = 4,
                        RoomType = RoomType.Deluxe
                    }
                }
            },
            new Hotel
            {
                HotelId = 2,
                Name = "Hotel B",
                Rooms = new List<Room>
                {
                    new Room
                    {
                        RoomId = 2,
                        HotelId = 2,
                        Capacity = 4,
                        RoomType = RoomType.Deluxe
                    }
                }
            }
        };

        mockHotelRepository.Setup(r => r.GetAll()).ReturnsAsync(hotels);

        var rooms = await bookingService.FindAvailable(DateOnly.Parse("2024-07-09"), DateOnly.Parse("2024-07-11"), 2);

        Assert.Equal(2, rooms.Length);
        Assert.Contains(rooms, r => r.HotelId == 1 && r.RoomId == 1);
        Assert.Contains(rooms, r => r.HotelId == 2 && r.RoomId == 2);
    }
}