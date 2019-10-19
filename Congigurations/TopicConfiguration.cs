using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySQLIdentity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySQLIdentity.Congigurations
{
    public class TopicConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {

            builder.HasAlternateKey(p => p.Name);//Property(p=>p.Name).IsRequired(true).IsUnicod
            //throw new NotImplementedException();
        }
    }
}
