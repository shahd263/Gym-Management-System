using GymManagementBLL;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Data.DataSeed;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace GymManagementPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                //options.UseSqlServer(builder. Configuration. GetSection("ConnectionStrings") ["DefaultConnection"]);
                //options.UseSqlServer(builder. Configuration["ConnectionStrings: DefaultConnection"]);

                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            });

            //builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); // DI Of GenericRepository That supports any Type of Entity
            //builder.Services.AddScoped<IPlanRepository, PlanRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddAutoMapper(X => X.AddProfile(new MappingProfiles()));


            var app = builder.Build();

            #region Data Seeding
            // using to close scope 
            using var Scope = app.Services.CreateScope();
            var dbContext = Scope.ServiceProvider.GetRequiredService<GymDbContext>();

            var PendingMigrations = dbContext.Database.GetPendingMigrations(); //To Check if there is any
                                                                               //pending migrates before seeding data
            if(PendingMigrations?.Any() ?? false)  
                dbContext.Database.Migrate();
            GymDbContextSeeding.SeedData(dbContext);



            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
