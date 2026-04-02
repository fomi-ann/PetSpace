using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetSpace.ApplicationServices.Services;
using PetSpace.Core.Domain;
using PetSpace.Core.ServiceInterface;
using PetSpace.Data;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PetSpaceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, UserRole>(options => {
    options.Password.RequireDigit = false;
    // PASSWORD LENGHT
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<PetSpaceDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<IPetSpaceServices, PetSpaceServices>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
