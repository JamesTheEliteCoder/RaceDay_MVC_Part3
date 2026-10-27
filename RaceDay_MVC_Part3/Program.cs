using RaceDay_MVC_Part3.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

builder.Services.AddHttpClient();

// Registers the API client so that controllers can communicate with the Part 2 API.
builder.Services.AddScoped<RaceDayApiClient>();

// Registers the in-memory cache required by ASP.NET Core session storage.
builder.Services.AddDistributedMemoryCache();

// Configures a separate session for each MVC website visitor.
builder.Services.AddSession(options =>
{
    // Ends the MVC session after 30 minutes of inactivity.
    options.IdleTimeout = TimeSpan.FromMinutes(30);

    // Prevents browser-side JavaScript from reading the MVC session cookie.
    options.Cookie.HttpOnly = true;

    // Allows the session cookie to be used when consent is required.
    options.Cookie.IsEssential = true;
});



// Adding of services to the container
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configuration of the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseRouting();

// Makes the MVC session available to controllers and other middleware.
app.UseSession();
app.UseAuthorization();


app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
