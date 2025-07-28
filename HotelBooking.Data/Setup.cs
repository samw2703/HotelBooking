using HotelBooking.Core.Repositories;
using HotelBooking.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBooking.Data;

public static class Setup
{
    public static void AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

        services.AddScoped<IHotelRepository, HotelRepository>();
    }

    public static void EnsureUpdated(this IServiceScope scope)
    {
        scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
    }
}