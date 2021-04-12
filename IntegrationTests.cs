using Covid19.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Covid19.DTO;
using Newtonsoft.Json;
using Covid19.Data.Interfaces;
using Covid19.Services.Interfaces;
using System.Net;
using System.Globalization;

namespace Covid19.Tests
{
    public class IntegrationTests
    {
        protected readonly HttpClient TestClient;

        public IntegrationTests()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {                    
                    builder.ConfigureServices(services =>
                    {
                        services.Remove(services.FirstOrDefault(d => d.ServiceType == typeof(IDatabaseSeeder)));
                        services.Remove(services.FirstOrDefault(d => d.ServiceType == typeof(INewsFeedService)));
                        services.RemoveAll(typeof(DbContextOptions<CovidContext>));

                        services.AddTransient<IDatabaseSeeder, TestSeeder>();
                        services.AddTransient<INewsFeedService, TestNewsFeedService>();

                        var serviceProvider = new ServiceCollection()
                            .AddEntityFrameworkInMemoryDatabase()
                            .BuildServiceProvider();

                        services.AddDbContextPool<CovidContext>(options => { 
                            options.UseInMemoryDatabase("Testing");
                            options.UseInternalServiceProvider(serviceProvider);
                        });

                        var sp = services.BuildServiceProvider();

                        using (var scope = sp.CreateScope())
                        {
                            var scopedServices = scope.ServiceProvider;
                            var db = scopedServices.GetRequiredService<CovidContext>();                            

                            db.Database.EnsureCreated();

                            var seeder = scopedServices.GetRequiredService<IDatabaseSeeder>();
                            seeder.SeedAsync().Wait();
                        }
                    });                    
                });
            TestClient = appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync(bool emailConfirmed)
        {
            string username;
            string password;

            if (emailConfirmed)
            {
                username = "IntTesting1";
                password = "Password!123";
            }
            else
            {
                username = "IntTesting2";
                password = "Password!123";
            }

            await LoginAsync(username, password);
        }

        protected async Task<LoginResponse> LoginAsync(string username, string password)
        {
            var loginResponse = await TestClient.PostAsJsonAsync("api/account/login", new LoginDTO() { Username = username, Password = password });
            var resCont = await loginResponse.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<LoginResponse>(resCont);
            responseContent.RefreshCookie = ReadRefreshCookie(loginResponse.Headers);

            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseContent.Token);

            return responseContent;
        }

        protected static Cookie ReadRefreshCookie(HttpResponseHeaders headers)
        {
            try
            {
                headers.TryGetValues("Set-Cookie", out var refreshCookieList);
                var refreshCookie = refreshCookieList.FirstOrDefault().Split(';', 6);
                var keyValue = refreshCookie[0].Split('=', 2);
                var date = refreshCookie[1].Split(' ', 7);
                var time = date[5].Split(':', 3);

                return new Cookie()
                {
                    Name = keyValue[0],
                    Value = keyValue[1],
                    Expires = new DateTime(int.Parse(date[4]), DateTime.ParseExact(date[3], "MMM", CultureInfo.CurrentCulture).Month, int.Parse(date[2]), int.Parse(time[0]), int.Parse(time[1]), int.Parse(time[2])),
                    Path = refreshCookie[2].Split('=', 2)[1],
                    Secure = refreshCookie[3] == " secure",
                    HttpOnly = refreshCookie[5] == " httponly"
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    public class LoginResponse
    {
        public AccountUserDTO User { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public Cookie RefreshCookie { get; set; }
    }
}
