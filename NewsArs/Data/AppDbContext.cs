﻿using Microsoft.EntityFrameworkCore;
using NewsArs.Models;
using System.Data;

namespace NewsArs.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Role adminRole = new Role { Id = 1, Name = "Admin" };
            Role writerRole = new Role { Id = 2, Name = "Writer" };
            User ownerUser = new User
            {
                Id = 1,
                Email = WC.DefaultAdminEmail,
                Password = WC.DefaultAdminPassword,
                FullName = WC.DefaultAdminFullName,
                RoleId = adminRole.Id
            };
            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, writerRole });
            modelBuilder.Entity<User>().HasData(new User[] { ownerUser });
            base.OnModelCreating(modelBuilder);
        }
    }
}
