using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GymManagementDAL.Data.Configurations
{
    internal class SessionBookingConfiguration : IEntityTypeConfiguration<SessionBooking>
    {
        public void Configure(EntityTypeBuilder<SessionBooking> builder)
        {
            builder.HasKey(X => new { X.MemberId, X.SessionId });
            builder.Ignore(X=>X.Id);

            builder.Property(X => X.CreatedAt)
                .HasColumnName("BookingDay")
                .HasDefaultValueSql("GETDATE()");


        }
    }
}
