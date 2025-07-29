using HotelBooking.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data;

internal class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(x => x.HotelId);
            entity.HasMany(x => x.Rooms).WithOne(x => x.Hotel).HasForeignKey(x => x.HotelId);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(x => x.RoomId);
            entity.HasMany(x => x.Bookings).WithOne(x => x.Room).HasForeignKey(x => x.RoomId);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(x => x.BookingId);
            entity.HasIndex(x => x.BookingReference).IsUnique();
            entity.HasIndex(x => x.IdempotencyKey).IsUnique();
        });
    }
}
