using DeviceManagement.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DeviceManagement.Tests.IntegrationTests
{
    /// <summary>
    /// Provides a custom implementation of WebApplicationFactory for integration testing, configuring the web host to
    /// use an in-memory database for the application's DbContext.
    /// </summary>
    /// <typeparam name="TProgram">The entry point class of the ASP.NET Core application to be tested.</typeparam>
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        private readonly string _databaseName = Guid.NewGuid().ToString();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DeviceDbContext));
                services.RemoveAll(typeof(DbContextOptions<DeviceDbContext>));
                services.RemoveAll(typeof(DbContextOptions));

                var options = new DbContextOptionsBuilder<DeviceDbContext>()
                    .UseInMemoryDatabase(_databaseName)
                    .Options;

                services.AddScoped<DeviceDbContext>(_ => new DeviceDbContext(options));
            });
        }
    }
}
