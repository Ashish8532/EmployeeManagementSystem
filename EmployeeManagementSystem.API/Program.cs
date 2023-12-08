using EmployeeManagementSystem.Data.Context;
using EmployeeManagementSystem.Domain.Helper;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Database connection string configuration
var connectionStrings = builder.Configuration.GetConnectionString("EMSApplicationEntities");
builder.Services.AddDbContextPool<EMSDbContext>(options => options.UseSqlServer(
connectionStrings, b => b.MigrationsAssembly("EmployeeManagementSystem.Data")));

// Adding AutoMapper with profiles defined in the project.
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

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
