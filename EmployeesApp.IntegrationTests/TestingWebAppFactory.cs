using System;
using System.Linq;
using EmployeesApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeesApp.IntegrationTests
{
    public class TestingWebAppFactory<T> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(opts =>
            {
                var descriptor = opts.SingleOrDefault(a => a.ServiceType == typeof(DbContextOptions<EmployeeContext>));

                if (descriptor is not null) opts.Remove(descriptor);

                var serviceProvider = new ServiceCollection()
                                        .AddEntityFrameworkInMemoryDatabase()
                                        .BuildServiceProvider();

                opts.AddDbContext<EmployeeContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryEmployeeTest");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                var sp = opts.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    using (var appContext = scope.ServiceProvider.GetRequiredService<EmployeeContext>())
                    {
                        try
                        {
                            appContext.Database.EnsureCreated();
                        }
                        catch (Exception ex)
                        {
                            //Log errors or do anything you think it's needed
                            throw new NotImplementedException($"Seeding Failed , {ex.Message}");
                        }
                    }
                }
            });
        }
    }
}
