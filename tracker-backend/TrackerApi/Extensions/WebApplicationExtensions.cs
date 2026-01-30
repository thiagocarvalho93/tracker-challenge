using Microsoft.AspNetCore.Builder;
using TrackerApi.Middlewares;

namespace TrackerApi.Extensions;

public static class WebApplicationExtensions
{
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

        return app;
    }
}