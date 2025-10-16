using BritInsurance.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BritInsurance.Api.IntegrationTests
{
    internal class BritInsuranceWebFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "TestScheme";
                    options.DefaultChallengeScheme = "TestScheme";
                })
                .AddScheme<AuthenticationSchemeOptions, AuthHandler>("TestScheme", _ => { });

                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<BritInsuranceDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                services.AddDbContext<BritInsuranceDbContext>(options =>
                {
                    options.UseSqlite(connection);
                });

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<BritInsuranceDbContext>();
                db.Database.EnsureCreated();
            });
        }
    }
}