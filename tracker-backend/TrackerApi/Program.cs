using System.Diagnostics.CodeAnalysis;
using TrackerApi.Extensions;

[ExcludeFromCodeCoverage]
class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServices();

        var app = builder.Build();

        app.RegisterMiddlewares();

        app.Run();
    }
}
