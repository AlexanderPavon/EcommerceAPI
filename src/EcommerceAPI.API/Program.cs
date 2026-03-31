using EcommerceAPI.Application;
using EcommerceAPI.Infrastructure;
using EcommerceAPI.Infrastructure.Services;
using Hangfire;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Seed roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    foreach (var role in new[] { "Admin", "Customer" })
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard("/hangfire");
app.MapControllers();

// Registrar job recurrente — limpieza de carritos cada día a medianoche
RecurringJob.AddOrUpdate<CartCleanupJob>(
    "cleanup-abandoned-carts",
    job => job.CleanAbandonedCartsAsync(),
    Cron.Daily);

app.Run();
