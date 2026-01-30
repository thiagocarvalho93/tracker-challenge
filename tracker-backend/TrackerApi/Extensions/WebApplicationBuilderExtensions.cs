using TrackerApi.Repositories;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services;
using TrackerApi.Services.Interfaces;
using TrackerApi.Utils;

namespace TrackerApi.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddHttpsRedirection(options =>
        {
            options.HttpsPort = 443;
        });

        // DI
        builder.Services.AddScoped<ITrackerService, TrackerService>();
        builder.Services.AddScoped<IPathRepository, PathRepository>();
        builder.Services.Configure<PathSettings>(
            builder.Configuration.GetSection("PathSettings"));

        // CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", policy =>
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod());
        });

        return builder;
    }
}