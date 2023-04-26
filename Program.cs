using Microsoft.EntityFrameworkCore;
using OrdersService.Business_Layer;
using OrdersService.Context;
using OrdersService.DB_Access;
using OrdersService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add dbContext to di container

builder.Services.AddDbContext<OrdersServiceContext>();
builder.Services.AddScoped<IDB_Provider, DB_Provider>();
builder.Services.AddScoped<IOrderService, OrderService>(); 
 var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrdersServiceContext>();
    db.Database.Migrate();
}

app.Run();
