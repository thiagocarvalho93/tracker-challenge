using Microsoft.AspNetCore.Builder;
using TrackerApi.Configuration;
using TrackerApi.Middlewares;
using TrackerApi.Repositories;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Extensions;

public static class WebApplicationExtensions
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

    public static WebApplication RegisterMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowAllOrigins");

        // app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        app.UseStaticFiles();
        app.MapFallbackToFile("index.html");

        return app;
    }
}