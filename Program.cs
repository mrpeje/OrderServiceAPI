using Microsoft.EntityFrameworkCore;
using OrdersService.Business_layer;
using OrdersService.Context;
using OrdersService.DB_Access;
using OrdersService.Business_layer.Validator;
using OrdersService.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add dbContext to di container

builder.Services.AddDbContext<OrdersServiceContext>();
builder.Services.AddScoped<IOrderRepository, DbOrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderValidator, OrderValidator>();
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
