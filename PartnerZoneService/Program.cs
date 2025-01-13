using System.Reflection;
using BaseServiceContracts.Consts;
using BaseServiceContracts.Interfaces.UnitOfWork;
using BaseServiceData.Contexts.PsSql;
using BaseServiceData.Repositories.UnitOfWork;
using EventBus.RMQ.Interfaces;
using Microsoft.EntityFrameworkCore;
using PartnerZoneService.Consumers.DI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContextFactory<PsSqlApplicationDataContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreDb"));
    opt.EnableThreadSafetyChecks();
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Register consumers into DI
builder.Services.AddPartnerZoneConsumers(builder.Configuration);

//Register mediatR into DI and configure pipeline
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(RmqHeadEndpoints).GetTypeInfo().Assembly);
    cfg.Lifetime = ServiceLifetime.Scoped;
});

//Add bus to dDI
builder.Services.AddSingleton<IEventBus, EventBus.RMQ.EventBus>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthorization();

app.MapControllers();

app.Run();