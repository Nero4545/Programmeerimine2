using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public abstract class ServiceTestBase : IDisposable
    {
        protected ApplicationDbContext DbContext { get; }

        protected ServiceTestBase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            DbContext = new ApplicationDbContext(options);
        }

        protected Batch CreateBatch(string code = "B001", string beerTypeName = "IPA")
        {
            return new Batch
            {
                Code = code,
                BatchDate = new DateTime(2024, 1, 15),
                BeerType = new BeerType { Name = beerTypeName }
            };
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
