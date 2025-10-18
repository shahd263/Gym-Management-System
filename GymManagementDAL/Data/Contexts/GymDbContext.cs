using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Contexts
{
    public class GymDbContext : DbContext
    {


        GymDbContext(DbContextOptions<GymDbContext> options) :base(options) { }

        // h7ot el Configuration fy mkan loose aktr (gwa el appsettings)

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server = SHAHD\\SQLEXPRESS ; Database = GymManagementSystem ; Trusted_Connection = true ; TrustServerCertificate = true");

        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SessionBooking> SessionBookings { get; set; }
        public DbSet<MemberPlan> MemberPlans { get; set; }






    }
}
