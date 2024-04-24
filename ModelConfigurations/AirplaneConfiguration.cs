namespace cia_aerea_api.ModelConfigurations

public class AirplaneConfiguration : IEntityTypeConfiguration<Airplane>
{
    public void Configure(EntityTypeBuilder<Airplane> builder) 
    {
        builder.ToTable("tb_airplanes");
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd();

        builder.Property(a => a.Manufacturer)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.Model)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.Prefix)
            .IsRequired()
            .HasMaxLength(10);
            
        builder.HasMany(a => a.Maintenances)
            .WithOne(m => m.Airplane)
            .HasForeignKey(m => m.AirplaneId);
    }
}
