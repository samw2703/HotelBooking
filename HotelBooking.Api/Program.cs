using HotelBooking.Core.Services.Hotels;
using HotelBooking.Core;
using HotelBooking.Data;
using HotelBooking.Core.Services.Bookings;
using HotelBooking.Core.DTOs;
using HotelBooking.Core.Exceptions;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCoreServices();
builder.Services.AddDataServices(builder.Configuration);
builder.Services.AddScoped<DataSeeder>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    scope.EnsureUpdated();
}

app.MapGet("/hotels", async (IHotelService hotelService) => Results.Ok(await hotelService.GetAll()))
.WithName("GetAllHotels")
.WithSummary("Get a list of all hotels")
.Produces<HotelDto[]>(StatusCodes.Status200OK)
.WithOpenApi();

app.MapGet("/hotels/{name}", async (string name, [FromServices] IHotelService hotelService) =>
{
    var hotel = await hotelService.FindByName(name);

    return hotel == null
        ? Results.NotFound("Hotel not found")
        : Results.Ok(hotel);
})
.WithName("GetHotelByName")
.WithSummary("Find a hotel by its name")
.WithDescription("Returns hotel details if a hotel with the given name exists.")
.Produces<HotelDto>(StatusCodes.Status200OK)
.Produces<string>(StatusCodes.Status404NotFound)
.WithOpenApi();

app.MapGet("/hotels/availability", async (DateOnly startDate, DateOnly endDate, int numberOfGuests, 
    [FromServices] IBookingService bookingService) =>
{
    if (numberOfGuests < 1)
        return Results.BadRequest("Number of guests must be greater than 0");

    return Results.Ok(await bookingService.FindAvailable(startDate, endDate, numberOfGuests));
})
.WithName("FindAvailableRooms")
.WithSummary("Find available rooms in a hotel for a given date range and guest count")
.WithDescription("Checks all rooms in a hotel and returns the ones available for the given dates and number of guests.")
.Produces<RoomDto[]>(StatusCodes.Status200OK)
.Produces<string>(StatusCodes.Status400BadRequest)
.WithOpenApi();

app.MapPost("/bookings", async (BookingRequestDto request, [FromServices] IBookingService bookingService) =>
{
    try
    {
        if (request.NumberOfGuests < 1)
            return Results.BadRequest("Number of guests must be greater than 0");

        var result = await bookingService.BookRoomAsync(request);

        return Results.Created($"/bookings/{result.BookingReference}", result);
    }
    catch (RoomNotFoundException)
    {
        return Results.NotFound("Room not found");
    }
    catch (RoomNotAvailableException)
    {
        return Results.BadRequest("Room not available");
    }
})
.WithName("BookRoom")
.WithSummary("Book a hotel room")
.WithDescription(@"Creates a new booking if one does not already exist with the same idempotency key. 
Clients must provide a unique `idempotencyKey` for each booking request. 
If the same key is sent again, the existing booking will be returned.")
.Produces<BookingResponseDto>(StatusCodes.Status201Created)
.Produces<string>(StatusCodes.Status400BadRequest)
.Produces<string>(StatusCodes.Status404NotFound)
.WithOpenApi();

app.MapGet("/bookings/{reference}", async (string reference, [FromServices] IBookingService bookingService) =>
{
    var details = await bookingService.GetBookingDetails(reference);

    return details == null
        ? Results.NotFound("Booking not found")
        : Results.Ok(details);
})
.WithName("GetBookingDetails")
.WithSummary("Get booking details")
.WithDescription("Retrieves booking information based on a booking reference.")
.Produces<BookingDetailsDto>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithOpenApi();

app.MapPost("/seed", async ([FromServices] DataSeeder seeder) =>
{
    await seeder.Seed();

    return Results.Ok();
})
.WithName("SeedData")
.Produces(StatusCodes.Status200OK)
.WithOpenApi();
app.MapPost("/reset", async ([FromServices] DataSeeder seeder) =>
{
    await seeder.Reset();

    return Results.Ok();
})
.WithName("ResetData")
.Produces(StatusCodes.Status200OK)
.WithOpenApi();

app.Run();