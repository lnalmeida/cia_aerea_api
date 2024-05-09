using Microsoft.EntityFrameworkCore;
using cia_aerea_api.ModelConfigurations;
using cia_aerea_api.Models;

namespace cia_aerea_api.Contexts;

public class CiaAereaContext : DbContext 
{
    private readonly IConfiguration _configuration;

    public CiaAereaContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<Airplane> Airplanes => Set<Airplane>();
    public DbSet<Pilot> Pilots => Set<Pilot>();
    public DbSet<Flight> Flights => Set<Flight>();
    public DbSet<Maintenance> Maintenances => Set<Maintenance>();
    public DbSet<FlightCancellation> FlightCancelations => Set<FlightCancellation>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("CiaAereaConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AirplaneConfiguration());
        modelBuilder.ApplyConfiguration(new FlightConfiguration());
        modelBuilder.ApplyConfiguration(new PilotConfiguration());
        modelBuilder.ApplyConfiguration(new MaintenanceConfiguration());
        modelBuilder.ApplyConfiguration(new FlightCancelationConfiguration());
    }
}
