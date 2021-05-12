using cw8.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw8.EfConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(e => e.UserID).HasName("User_PK");
            builder.Property(e => e.Login).IsRequired().HasMaxLength(100);
            builder.Property(e => e.PasswordHashed).IsRequired().HasMaxLength(200);
            builder.Property(e => e.UserRole).IsRequired().HasMaxLength(200);
            builder.Property(e => e.Salt).IsRequired().HasMaxLength(200);
        }
    }
}
