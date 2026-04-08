using DeviceManagement.Api.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;

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

                services.AddAuthentication("TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });
            });
        }
    }

    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder)
            : base(options, logger, encoder) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "1"),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Email, "admin@test.com")
            };

            var identity = new ClaimsIdentity(claims, "TestScheme");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "TestScheme");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
