using cia_aerea_api.Contexts;
using cia_aerea_api.Repositories;
using cia_aerea_api.Validators.Airplanes;
using cia_aerea_api.Validators.Cancellation;
using cia_aerea_api.Validators.Flights;
using cia_aerea_api.Validators.Maintenances;
using cia_aerea_api.Validators.Pilots;
using cia_aerea_api.Validators.Services;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//context
builder.Services.AddDbContext<CiaAereaContext>();
//repositories
builder.Services.AddScoped<AirplaneRepository>();
builder.Services.AddScoped<PilotRepository>();
builder.Services.AddScoped<FlightRepository>();
builder.Services.AddScoped<MaintenanceRepository>();
//validators
builder.Services.AddScoped<AddAirplaneValidator>();
builder.Services.AddScoped<UpdateAirplaneValidator>();
builder.Services.AddScoped<DeleteAirplaneValidator>();
builder.Services.AddScoped<AddPilotValidator>();
builder.Services.AddScoped<UpdatePilotValidator>();
builder.Services.AddScoped<DeletePilotValidator>();
builder.Services.AddScoped<ValidationService>();
builder.Services.AddScoped<AddFlightValidator>();
builder.Services.AddScoped<UpdateFlightValidator>();
builder.Services.AddScoped<DeleteFlightValidator>();
builder.Services.AddScoped<FlightCancellationValidator>();
builder.Services.AddScoped<AddMaintenanceValidator>();
builder.Services.AddScoped<UpdateMaintenanceValidator>();
builder.Services.AddScoped<DeleteMaintenanceValidator>();
//injecting IConverter form DinkToPDF
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
