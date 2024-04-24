namespace cia_aerea_api.Models.Contexts

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
    public DbSet<FlightCancelation> FlightCancelations => Set<FlightCancelation>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("CiaAereaConnection"));
    }
}
