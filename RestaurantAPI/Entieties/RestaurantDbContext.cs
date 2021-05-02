using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Entieties
{
   public class RestaurantDbContext : DbContext
   {
      private string _connectionString = "Server=localhost\\SQLEXPRESS;Database=RestaurantDatabase;Trusted_Connection=True;";
      public DbSet<Restaurant> Restaurants { get; set; }
      public DbSet<Address> Addresses { get; set; }
      public DbSet<Dish> Dishes { get; set; }
      public DbSet<User> Users { get; set; }
      public DbSet<Role> Roles { get; set; }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         modelBuilder.Entity<User>()
            .Property(u => u.Email).IsRequired()
            .IsRequired();

         modelBuilder.Entity<Role>()
            .Property(u => u.Name).IsRequired()
            .IsRequired();

         modelBuilder.Entity<Restaurant>()
            .Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(25);

         modelBuilder.Entity<Dish>()
            .Property(d => d.Name)
            .IsRequired();

         modelBuilder.Entity<Address>()
            .Property(c => c.City)
            .IsRequired()
            .HasMaxLength(50);

         modelBuilder.Entity<Address>()
            .Property(s => s.Street).IsRequired()
            .IsRequired()
            .HasMaxLength(50); 
      }
      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
         optionsBuilder.UseSqlServer(_connectionString);
      }
   }
}
