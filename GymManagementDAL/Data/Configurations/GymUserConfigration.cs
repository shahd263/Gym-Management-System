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
    internal class GymUserConfigration<T> : IEntityTypeConfiguration<T> where T : GymUser

    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(X => X.Name)
                 .HasColumnType("varchar")
                 .HasMaxLength(50);

            builder.Property(X => X.Email)
                .HasColumnType("varchar")
                .HasMaxLength(100);


            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("GymUserValidEmailCheck", "(Email LIKE '_%@_%._%')");
                Tb.HasCheckConstraint("GymUserValidPhoneCheck", "(Phone LIKE '01%' AND Phone NOT LIKE '%[^0-9]%')");
            });

            builder.HasIndex(X => X.Email).IsUnique();
            builder.HasIndex(X => X.Phone).IsUnique();  


            builder.Property(X => X.Phone)
                .HasColumnType("varchar")
                .HasMaxLength(11); 

            


            builder.OwnsOne(A => A.Address, AddressBuilder =>
            {
                AddressBuilder.Property(A=>A.Street).HasColumnType("varchar").HasMaxLength(30);
                AddressBuilder.Property(A=>A.City).HasColumnType("varchar").HasMaxLength(30);
            });
           
;

        }
    }
}
