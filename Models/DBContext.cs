using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MySQLIdentity.Congigurations;
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
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new TopicConfiguration());
            builder.ApplyConfiguration(new PlaceConfiguration());
            builder.ApplyConfiguration(new LocationConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
        }


    }
}
