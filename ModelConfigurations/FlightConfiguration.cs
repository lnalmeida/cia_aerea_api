using cia_aerea_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cia_aerea_api.ModelConfigurations;

public class FlightConfiguration :IEntityTypeConfiguration<Flight>
{
     public void Configure(EntityTypeBuilder<Flight> builder) 
    {
        builder.ToTable("tb_flights");
        builder.HasKey(f => f.Id);
        
        builder.Property(f => f.Id)
            .ValueGeneratedOnAdd();

        builder.Property(f => f.Origin)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(f => f.Destiny)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(f => f.DepartureDateTime)
            .IsRequired();

        builder.Property(f => f.ArrivalDateTime)
            .IsRequired();

        builder.HasOne(f => f.Pilot)
            .WithMany(p => p.Flights)
            .HasForeignKey(f => f.PilotId);

        builder.HasOne(f => f.Airplane)
            .WithMany(a => a.Flights)
            .HasForeignKey(f => f.AirplaneId); 

        builder.HasOne(f => f.FlightCancelations)
            .WithOne(fc => fc.Flight)
            .HasForeignKey<FlightCancellation>(fc => fc.FlightId);
    }
}
