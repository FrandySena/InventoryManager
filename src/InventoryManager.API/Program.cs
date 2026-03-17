using InventoryManager.API.Models;
using InventoryManager.Application.Services;
using InventoryManager.Domain.Entities;
using InventoryManager.Domain.Entities.Catalogs;
using InventoryManager.Infrastructure.Repositories;
using InventoryManager.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InventoryManagerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("InventoryManagerConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
}, typeof(Program).Assembly);

builder.Services.AddTransient<ProductRepository>();
builder.Services.AddTransient<GenericRepository<Category>>();
builder.Services.AddTransient<GenericRepository<InventoryMovement>>();
builder.Services.AddTransient<UnitOfWork>();
builder.Services.AddTransient<CatalogService>();
builder.Services.AddTransient<InventoryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
