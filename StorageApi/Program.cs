
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StorageApi.Data;

namespace StorageApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        // Register DbContext with LocalDB
        builder.Services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));




        // Add services to the container.
        builder.Services.AddControllers();






        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
