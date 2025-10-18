using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configurations
{
    internal class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("SessionValidCapacitycheck", "Capacity between 1 and 25");
                Tb.HasCheckConstraint("SessionValidDateCheck" , "StartDate < EndDate");
            });

            builder.HasOne(X=>X.Category)
                .WithMany(X => X.Sessions)
                .HasForeignKey(X => X.CategoryId);

            builder.HasOne(X => X.Trainer)
                .WithMany(X => X.Sessions)
                .HasForeignKey(X=>X.TrainerId);


        }



    }
}
