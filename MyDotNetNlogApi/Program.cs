using NLog;
using NLog.Web;

// We need:
// Install-Package NLog -Version 5.1.0
// Install-Package NLog.Database -Version 5.1.0
// Install-Package NLog.Web.AspNetCore -Version 5.2.0
// Install-Package System.Data.SqlClient -Version 4.8.5
// Install-Package Swashbuckle.AspNetCore -Version 6.4.0

// Crate the Logger instance from the AppSettings appsettings.json file
Logger logger = NLog.LogManager
                    .Setup()
                    .LoadConfigurationFromAppSettings()
                    .GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddControllersWithViews();
    builder.Services.AddSwaggerGen();

    // Cleaer the build in provider
    builder.Logging.ClearProviders();

    // log youe application at trace level 
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);

    //Allow cors origin (CORS Policy)
    builder.Services.AddCors(options =>
    options.AddPolicy("CorsPolicy",
    builder => builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()));

    // Register the NLog service
    builder.Host.UseNLog();

    // Example for loggings
    logger.Trace("Trace log");
    logger.Debug("Debug log");
    logger.Info("Info log");
    logger.Warn("Warn log");
    logger.Error("Error log");

    // Add services to the container.
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Example Log with a property
    string user = "User 01";
    int filter = 1;
    logger.WithProperty("AppUser", user).Info("Message X");
    logger.WithProperty("Filter", filter).Info("Message Y");

    WebApplication app = builder.Build();
    app.UseStaticFiles();
    app.UseRouting();

    // (CORS Policy)
    app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    app.UseCors("CorsPolicy");

    app.MapDefaultControllerRoute();

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

}

catch (Exception ex)
{
    logger.Error(ex);
    throw;
}
finally
{
    // Ensure to shout downon the NLog ( Disposing )
    NLog.LogManager.Shutdown();
}