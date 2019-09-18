using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class BlogContext : DbContext
    {
        public BlogContext()
            : base("name=BlogContext")
        {}

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Skill> Skills { get; set; }
        //public DbSet<AdminProfile> AdminsProfiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //name bridge table between admin and skills 
            modelBuilder.Entity<Administrator>()
                .HasMany(a => a.Skills)
                .WithMany(s => s.Administrators)
                .Map
                (m => m.ToTable("AdministratorSkills"));
        }
    }
}