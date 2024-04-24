using cia_aerea_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cia_aerea_api.ModelConfigurations;

public class FlightCancelationConfiguration : IEntityTypeConfiguration<FlightCancelation>
{
    public void Configure(EntityTypeBuilder<FlightCancelation> builder) 
    {
        builder.ToTable("tb_flight_cancelations");
        builder.HasKey(fc => fc.Id);
        
        builder.Property(fc => fc.Id)
            .ValueGeneratedOnAdd();

        builder.Property(fc => fc.CancelationReason)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(fc => fc.NotificationDateTime)
            .IsRequired();
                    
    }
}
