using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySQLIdentity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySQLIdentity.Congigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<IdentityUser>
    {
        public void Configure(EntityTypeBuilder<IdentityUser> builder)
        {
            //builder.Property(p => p.Id).HasMaxLength(127);
            builder.HasAlternateKey(p => p.Email);
            builder.Property(p => p.Email).HasMaxLength(127);
            builder.HasAlternateKey(p => p.UserName);
            builder.Property(p => p.UserName).HasMaxLength(127);

            //throw new NotImplementedException();
        }

        
    }
}
