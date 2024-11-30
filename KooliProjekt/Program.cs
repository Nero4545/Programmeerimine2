using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<IOilTypeService, OilTypeService>();
builder.Services.AddScoped<ISampleBatchService, SampleBatchService>();
builder.Services.AddScoped<ILogEntryService, LogEntryService>(); 
builder.Services.AddScoped<IManufacturingLogService, ManufacturingLogService>();  
builder.Services.AddScoped<IBatchCompositionService, BatchCompositionService>();  
builder.Services.AddScoped<IOilBatchService, OilBatchService>();  
builder.Services.AddScoped<IProductComponentService, ProductComponentService>();
builder.Services.AddScoped<IOilTypeService, OilTypeService>();

namespace KooliProjekt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

       
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

       
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();

         
                using (var scope = app.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    SeedData.Generate(context); 
                }
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                    https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
