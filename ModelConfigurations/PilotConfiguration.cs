using cia_aerea_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cia_aerea_api.ModelConfigurations;

public class PilotConfiguration : IEntityTypeConfiguration<Pilot>
{
    public void Configure(EntityTypeBuilder<Pilot> builder) 
    {
        builder.ToTable("tb_pilots");
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Registration)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasIndex(p => p.Registration)
            .IsUnique();
                    
    }
}