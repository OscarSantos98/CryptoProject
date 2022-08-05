using CryptoAPI.Models;
using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup>
     : WebApplicationFactory<TStartup> where TStartup : Program
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<CryptoDBContext>));

                services.Remove(descriptor);

                services.AddDbContext<CryptoDBContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });
                //services.AddDbContext<CryptoDBContext>(options =>
                //{
                //    options.UseSqlServer("Server=INVFRA000698\\SQLEXPRESS;Database=IntegrationTestDB;Trusted_Connection=True;");
                //});

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<CryptoDBContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        Utilities.InitializeDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }


                services.AddMvc(option => option.EnableEndpointRouting = false)
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);


                //----------------------------- adding Service Configuration from Program.cs
                //// Add services to the container.

                //services.AddControllers();

                //// The hhtpclient factory
                //services.AddHttpClient();

                ////  register the DB Contexr with the dependency injection container
                ////services.AddDbContext<CryptoDBContext>(options =>
                ////{
                ////    options.UseSqlServer(Configuration.GetConnectionString("CryptoDBTesting"));
                ////});
                //services.AddMvc(option => option.EnableEndpointRouting = false)
                //                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                //                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
                ////to validate the token which has been sent by clients
                //var key = Encoding.ASCII.GetBytes("SecretKeyAnassIntegrationTestingSecretKey");

                //services.AddAuthentication(x =>
                //{
                //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //})
                //.AddJwtBearer(x =>
                //{
                //    x.RequireHttpsMetadata = true;
                //    x.SaveToken = true;
                //    x.TokenValidationParameters = new TokenValidationParameters
                //    {
                //        ValidateIssuerSigningKey = true,
                //        IssuerSigningKey = new SymmetricSecurityKey(key),
                //        ValidateIssuer = false,
                //        ValidateAudience = false,
                //        ClockSkew = TimeSpan.Zero
                //    };
                //});
                //-------------------------------- Trying to seed data in .Configure
            });
        }
    }
}
