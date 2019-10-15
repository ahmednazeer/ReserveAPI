using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MySQLIdentity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySQLIdentity
{
    public class DBContext:IdentityDbContext
    {
        public DBContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<User> _Users { get; set; }
        public DbSet<Category> Categories { get; set; }

        /*
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<IdentityUser>().HasKey(p => p.Id);
            builder.Entity<User>().Property(p => p.Id)
            .ValueGeneratedOnAdd();
            //base.OnModelCreating(builder);
        }*/


    }
}
