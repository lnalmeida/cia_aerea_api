using cia_aerea_api.Contexts;
using cia_aerea_api.Repositories;
using cia_aerea_api.Validators.Airplanes;
using cia_aerea_api.Validators.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//context
builder.Services.AddDbContext<CiaAereaContext>();
//repositories
builder.Services.AddScoped<AirplaneRepository>();
//validators
builder.Services.AddScoped<AddAirplaneValidator>();
builder.Services.AddScoped<UpdateAirplaneValidator>();
builder.Services.AddScoped<DeleteAirplaneValidator>();
builder.Services.AddScoped<ValidationService>();

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
