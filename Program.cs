using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    if (!context.Countries.Any())
    {
        var india = new Country
        {
            Name = "India"
        };

        var usa = new Country
        {
            Name = "USA"
        };

        context.Countries.AddRange(india, usa);
        context.SaveChanges();

        var assam = new State
        {
            Name = "Assam",
            CountryId = india.Id
        };

        var westBengal = new State
        {
            Name = "West Bengal",
            CountryId = india.Id
        };

        var california = new State
        {
            Name = "California",
            CountryId = usa.Id
        };

        context.States.AddRange(
            assam,
            westBengal,
            california
        );

        context.SaveChanges();

        context.Cities.AddRange(
            new City
            {
                Name = "Guwahati",
                StateId = assam.Id
            },
            new City
            {
                Name = "Dibrugarh",
                StateId = assam.Id
            },
            new City
            {
                Name = "Kolkata",
                StateId = westBengal.Id
            },
            new City
            {
                Name = "Los Angeles",
                StateId = california.Id
            },
            new City
            {
                Name = "San Francisco",
                StateId = california.Id
            }
        );

        context.SaveChanges();
    }
}

app.Run();