using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MillionApi.Api;
using MillionApi.Infrastructure.SeedData;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        
        // Seed data
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            await MongoDbSeedData.SeedAsync(services);
        }
        
        await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
}