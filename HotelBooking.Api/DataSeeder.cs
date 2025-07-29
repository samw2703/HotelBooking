using HotelBooking.Core.Entities;
using HotelBooking.Core.Repositories;

class DataSeeder(IHotelRepository hotelRepository)
{ 
    public async Task Seed()
    {
        var hotels = await hotelRepository.GetAll();

        if (hotels.Any())
            return;

        var hotel1 = new Hotel
        {
            Name = "Mediocre Inn",
            Rooms = new List<Room>
            {
                new Room { RoomType = RoomType.Single, Capacity = 1 },
                new Room { RoomType = RoomType.Single, Capacity = 1 },
                new Room { RoomType = RoomType.Double, Capacity = 2 },
                new Room { RoomType = RoomType.Double, Capacity = 2 },
                new Room { RoomType = RoomType.Deluxe, Capacity = 4 },
                new Room { RoomType = RoomType.Deluxe, Capacity = 4 },
            }
        };
        var hotel2 = new Hotel
        {
            Name = "Worst Western",
            Rooms = new List<Room>
            {
                new Room { RoomType = RoomType.Single, Capacity = 1 },
                new Room { RoomType = RoomType.Single, Capacity = 1 },
                new Room { RoomType = RoomType.Double, Capacity = 2 },
                new Room { RoomType = RoomType.Double, Capacity = 2 },
                new Room { RoomType = RoomType.Deluxe, Capacity = 4 },
                new Room { RoomType = RoomType.Deluxe, Capacity = 4 },
            }
        };

        await hotelRepository.Save(hotel1);
        await hotelRepository.Save(hotel2);
    }

    public async Task Reset()
    {
        var hotels = await hotelRepository.GetAll();

        foreach (var hotel in hotels)
        {
            await hotelRepository.Delete(hotel);
        }
    }
}