using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace KooliProjekt.IntegrationTests.Helpers
{
    public abstract class TestBase : IDisposable
    {
        public WebApplicationFactory<FakeStartup> Factory { get; }

        private IServiceScope _scope;

        public TestBase()
        {
            Factory = new TestApplicationFactory<FakeStartup>();
        }

        protected HttpClient CreateClient()
        {
            return Factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        }

        protected ApplicationDbContext DbContext
        {
            get
            {
                _scope ??= Factory.Services.CreateScope();
                return _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            }
        }

        protected BeerType AddBeerType(string name = "IPA")
        {
            var beerType = new BeerType { Name = name };
            DbContext.BeerTypes.Add(beerType);
            DbContext.SaveChanges();
            return beerType;
        }

        protected Batch AddBatch(string code = "B001")
        {
            var batch = new Batch
            {
                Code = code,
                BatchDate = new DateTime(2024, 1, 15),
                BeerType = new BeerType { Name = "Type " + code }
            };
            DbContext.Batches.Add(batch);
            DbContext.SaveChanges();
            return batch;
        }

        protected void EnsureTestUser()
        {
            if (DbContext.Users.Find(TestAuthHandler.UserId) != null)
            {
                return;
            }

            DbContext.Users.Add(new IdentityUser
            {
                Id = TestAuthHandler.UserId,
                UserName = "test@pruulikoda.ee",
                NormalizedUserName = "TEST@PRUULIKODA.EE"
            });
            DbContext.SaveChanges();
        }

        protected static async Task<HttpResponseMessage> PostFormAsync(HttpClient client, string formUrl, string postUrl, Dictionary<string, string> values)
        {
            var page = await client.GetStringAsync(formUrl);
            var match = Regex.Match(page, "name=\"__RequestVerificationToken\"[^>]*value=\"([^\"]+)\"");
            if (match.Success)
            {
                values["__RequestVerificationToken"] = match.Groups[1].Value;
            }

            using var content = new FormUrlEncodedContent(values);
            return await client.PostAsync(postUrl, content);
        }

        public void Dispose()
        {
            _scope?.Dispose();

            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureDeleted();

            Factory.Dispose();
        }
    }
}
