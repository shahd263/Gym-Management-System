﻿using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configurations
{
    internal class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(X => X.Name)
                .HasColumnType("varchar").HasMaxLength(50);


            builder.Property(X => X.Description)
               .HasColumnType("varchar").HasMaxLength(200);

            builder.Property(X => X.Price).HasPrecision(10, 2);

            builder.ToTable(Tb => {
                Tb.HasCheckConstraint("PlanValidDurationDaysCheck", "DurationDays between 1 and 365");
                });
        }
    }
}
