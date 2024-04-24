namespace cia_aerea_api.ModelConfigurations

public class FlightCancelationConfigure : IEntityTypeConfiguration<Maintenance>
{
    public void Configure(EntityTypeBuilder<Maintenance> builder) 
    {
        builder.ToTable("tb_maintenances");
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.Id)
            .ValueGeneratedOnAdd();

        builder.Property(m => m.MaintenanceDateTime)
            .IsRequired();

        builder.Property(m => m.TypeOfMaintenance)
            .IsRequired();

        builder.Property(m => m.Comments)
            .HasMaxLength(100);
                    
    }
}
