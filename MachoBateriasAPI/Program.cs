using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MachoBateriasAPI.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MachoBateriasAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MachoBateriasAPIContext") ?? throw new InvalidOperationException("Connection string 'MachoBateriasAPIContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(p => p.AddPolicy("MyCorsPolicy", build =>
{
    build.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
}));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("MyCorsPolicy");
app.MapControllers();

app.Run();
