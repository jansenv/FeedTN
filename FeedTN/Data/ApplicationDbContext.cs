using System;
using System.Collections.Generic;
using System.Text;
using FeedTN.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FeedTN.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Favorite> Favorite { get; set; }
        public DbSet<MenuItem> MenuItem { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Report> Report { get; set; }
        public DbSet<School> School { get; set; }
        public DbSet<UserMenuItem> UserMenuItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("GETDATE()");

            ApplicationUser user = new ApplicationUser
            {
                FirstName = "Admina",
                LastName = "Straytor",
                Address = "123 Infinity Way",
                City = "Nashville",
                State = "TN",
                Zip = "37013",
                SchoolId = 1,
                IsAdmin = true,
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = "7f434309-a4d9-48e9-9ebb-8803db794577",
                Id = "00000000-ffff-ffff-ffff-ffffffffffff"
            };
            var passwordHash = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHash.HashPassword(user, "Admin8*");
            modelBuilder.Entity<ApplicationUser>().HasData(user);

            modelBuilder.Entity<School>().HasData(
                new School()
                {
                    SchoolId = 1,
                    Name = "Cane Ridge Elementary",
                    Address = "3884 Asheford Trace",
                    City = "Antioch",
                    State = "TN",
                    Zip = "37013"
                },
                new School()
                {
                    SchoolId = 2,
                    Name = "Dupont Elementary",
                    Address = "1311 9th St",
                    City = "Old Hickory",
                    State = "TN",
                    Zip = "37138"
                },
                new School()
                {
                    SchoolId = 3,
                    Name = "Eagle View Elementary",
                    Address = "1470 Eagle View Blvd",
                    City = "Antioch",
                    State = "TN",
                    Zip = "37013"
                },
                new School()
                {
                    SchoolId = 4,
                    Name = "Glendale Elementary",
                    Address = "800 Thompson Ave",
                    City = "Nashville",
                    State = "TN",
                    Zip = "37204"
                },
                new School()
                {
                    SchoolId = 5,
                    Name = "Goodlettsville Elementary",
                    Address = "514 Donald Ave",
                    City = "Goodlettsville",
                    State = "TN",
                    Zip = "37072"
                },
                new School()
                {
                    SchoolId = 6,
                    Name = "Haywood Elementary",
                    Address = "3790 Turley Dr",
                    City = "Nashville",
                    State = "TN",
                    Zip = "37211"
                },
                new School()
                {
                    SchoolId = 7,
                    Name = "McGavock Elementary",
                    Address = "275 McGavock Pk",
                    City = "Nashville",
                    State = "TN",
                    Zip = "37211"
                },
                new School()
                {
                    SchoolId = 8,
                    Name = "RePublic High",
                    Address = "3307 Brick Church Pike",
                    City = "Nashville",
                    State = "TN",
                    Zip = "37207"
                },
                new School()
                {
                    SchoolId = 9,
                    Name = "Smith Springs Elementary",
                    Address = "3132 Smith Springs Rd",
                    City = "Antioch",
                    State = "TN",
                    Zip = "37013"
                },
                new School()
                {
                    SchoolId = 10,
                    Name = "Westmeade Elementary",
                    Address = "6641 Clearbrook Dr",
                    City = "Nashville",
                    State = "TN",
                    Zip = "37205"
                }
            );

            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem
                {
                    MenuItemId = 1,
                    Title = "Kale Salad",
                    Description = "400 calories",
                    ImagePath = "kale.jpg",
                    FavoriteCount = 32,
                    Active = true,
                    GlutenFree = true,
                    Vegetarian = true,
                    Vegan = true
                },
                new MenuItem
                {
                    MenuItemId = 2,
                    Title = "Vegan Burrito Bowl",
                    Description = "800 calories",
                    ImagePath = "burritoBowl.jpg",
                    FavoriteCount = 55,
                    Active = true,
                    GlutenFree = false,
                    Vegetarian = true,
                    Vegan = true
                },
                new MenuItem
                {
                    MenuItemId = 3,
                    Title = "5 Nuggets",
                    Description = "80 calories per nugget",
                    ImagePath = "nuggets.jpg",
                    FavoriteCount = 45,
                    Active = true,
                    GlutenFree = false,
                    Vegetarian = false,
                    Vegan = false
                },
                new MenuItem
                {
                    MenuItemId = 4,
                    Title = "Square Pizza",
                    Description = "200 calories per slice",
                    ImagePath = "pizza.jpg",
                    FavoriteCount = 124,
                    Active = true,
                    GlutenFree = true,
                    Vegetarian = true,
                    Vegan = false
                },
                new MenuItem
                {
                    MenuItemId = 5,
                    Title = "Chocolate Milk",
                    Description = "90 calories",
                    ImagePath = "chocMilk.jpg",
                    FavoriteCount = 44,
                    Active = true,
                    GlutenFree = true,
                    Vegetarian = true,
                    Vegan = false
                },
                new MenuItem
                {
                    MenuItemId = 6,
                    Title = "Orange Juice",
                    Description = "60 calories",
                    ImagePath = "oj.jpg",
                    FavoriteCount = 23,
                    Active = true,
                    GlutenFree = true,
                    Vegetarian = true,
                    Vegan = true
                }
            );
        }
    }
}
