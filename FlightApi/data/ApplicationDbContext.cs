using FlightApi.Model;
using Microsoft.EntityFrameworkCore;

namespace FlightApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Airport> Airports => Set<Airport>();
    public DbSet<Flight> Flights => Set<Flight>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Username).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
        });

        modelBuilder.Entity<Airport>(entity =>
        {
            entity.Property(a => a.IataCode).HasMaxLength(3).IsFixedLength();
            entity.HasIndex(a => a.IataCode).IsUnique();
            entity.Property(a => a.Country).HasDefaultValue("Vietnam");
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.Property(f => f.NatureOfFlight).HasDefaultValue("---");
            entity.Property(f => f.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

            entity.HasOne(f => f.OriginAirport)
                .WithMany(a => a.OriginFlights)
                .HasForeignKey(f => f.OriginAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(f => f.DestinationAirport)
                .WithMany(a => a.DestinationFlights)
                .HasForeignKey(f => f.DestinationAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(f => new
            {
                f.FlightNo,
                f.FlightDate,
                f.OriginAirportId,
                f.DestinationAirportId,
                f.ArrDep
            }).IsUnique();

            entity.HasIndex(f => f.FlightDate);
            entity.HasIndex(f => f.Status);

            entity.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Flights_ArrDep", "[ArrDep] IN ('A', 'D')");
                t.HasCheckConstraint("CK_Flights_Status", "[Status] IN ('OPN', 'DLY', 'CNX', 'CLS', 'XXX')");
                t.HasCheckConstraint("CK_Flights_FlightType", "[FlightType] IN ('PAX', 'CGO')");
                t.HasCheckConstraint("CK_Flights_OriginDest", "[OriginAirportId] <> [DestinationAirportId]");
            });
        });
    }
}
