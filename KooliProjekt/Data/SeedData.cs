using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace KooliProjekt.Data
{
    [ExcludeFromCodeCoverage]
    public static class SeedData
    {
        public static void Generate(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            if (context.BeerTypes.Any())
            {
                return;
            }

            var user = new IdentityUser
            {
                UserName = "admin@pruulikoda.ee",
                Email = "admin@pruulikoda.ee",
                EmailConfirmed = true
            };
            userManager.CreateAsync(user, "Password123!").GetAwaiter().GetResult();

            var beerNames = new[]
            {
                "Pale Ale", "IPA", "Stout", "Porter", "Pilsner",
                "Lager", "Wheat Beer", "Saison", "Sour Ale", "Amber Ale"
            };

            var units = new[] { "kg", "g", "l", "tk" };
            var ingredientNames = new[]
            {
                "Pale malt", "Crystal malt", "Cascade hops", "Saaz hops", "Ale yeast",
                "Lager yeast", "Wheat malt", "Roasted barley", "Water", "Sugar"
            };

            var random = new Random(42);
            var startDate = new DateTime(2024, 1, 15);

            for (var i = 0; i < beerNames.Length; i++)
            {
                var beerType = new BeerType
                {
                    Name = beerNames[i],
                    Description = "Proovisort: " + beerNames[i]
                };

                var batch = new Batch
                {
                    BeerType = beerType,
                    BatchDate = startDate.AddDays(i * 14),
                    Code = "B" + (i + 1).ToString("000"),
                    Description = "Esimene proovipartii - " + beerNames[i],
                    Summary = i % 3 == 0 ? "Partii on valmis, maitse on tasakaalus." : null
                };

                var ingredient = new Ingredient
                {
                    Name = ingredientNames[i],
                    Unit = units[i % units.Length],
                    UnitPrice = Math.Round((decimal)(random.NextDouble() * 10 + 1), 2),
                    Quantity = random.Next(1, 20)
                };
                ingredient.Total = Math.Round(ingredient.UnitPrice * ingredient.Quantity, 2);
                batch.Ingredients.Add(ingredient);

                batch.LogEntries.Add(new LogEntry
                {
                    Date = batch.BatchDate,
                    Description = "Pruulimine alustatud, käärimine käib.",
                    User = user
                });

                batch.TastingEntries.Add(new TastingEntry
                {
                    Date = batch.BatchDate.AddDays(21),
                    User = user,
                    Rating = random.Next(4, 11),
                    Comment = "Maitsmine pärast laagerdumist."
                });

                batch.Photos.Add(new BatchPhoto
                {
                    Url = "https://example.com/photos/" + batch.Code + ".jpg",
                    Description = "Partii " + batch.Code + " foto"
                });

                beerType.Batches.Add(batch);
                context.BeerTypes.Add(beerType);
            }

            context.SaveChanges();
        }
    }
}
