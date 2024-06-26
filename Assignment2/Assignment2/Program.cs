﻿using System.Globalization;
using Hangfire.AspNetCore;
using DataModelLibrary.Data;
using Assignment2.Services;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;


// Set the default culture to Australian English.
var defaultCulture = new CultureInfo("en-AU");
CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Assignment2Context");
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<McbaContext>(options =>
{
    options.UseSqlServer(connectionString);

    // Enable lazy loading.
    options.UseLazyLoadingProxies();
});

// Store session into Web-Server memory.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    // Make the session cookie essential.
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();

// add hangfire services
builder.Services.AddHangfire((sp, config) =>
{
    config.UseSqlServerStorage(connectionString);
});

builder.Services.AddHangfireServer();

var app = builder.Build();

// Seed data.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<McbaContext>();
        SeedData.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseHangfireDashboard();

app.UseSession();

app.MapDefaultControllerRoute();


// Bill pay runs every minute
RecurringJob.AddOrUpdate<BillPayService>(x => x.PayScheduledBills(), "* * * * *");

app.Run();


