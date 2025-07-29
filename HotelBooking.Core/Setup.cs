using HotelBooking.Core.Services.Bookings;
using HotelBooking.Core.Services.Hotels;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBooking.Core;

public static class Setup
{
    public static void AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IHotelService, HotelService>();
        services.AddScoped<IBookingService, BookingService>();
    }
}