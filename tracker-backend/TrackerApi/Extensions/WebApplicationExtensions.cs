using Microsoft.AspNetCore.Builder;

namespace TrackerApi.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication RegisterMiddlewares(this WebApplication app)
    {
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