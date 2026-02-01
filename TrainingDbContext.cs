using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectSGMS
{
    public class TrainingDbContext : DbContext
    {
       
            private readonly string _connectionString;

            public TrainingDbContext()
            {
                _connectionString = "Server = .\\SQLEXPRESS;Initial Catalog=CSharpB16Final;User ID=csharpb16;Password=123456;TrustServerCertificate=True; ";
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                if (!optionsBuilder.IsConfigured)
                    optionsBuilder.UseSqlServer(_connectionString);

                base.OnConfiguring(optionsBuilder);
            }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().ToTable("UsersTable");
            modelBuilder.Entity<Users>().HasData
                (
                     new Users() 
                     {
                       Id = Guid.Parse("414AF32C-F8E8-4787-86BF-1995D43B6446"),
                       Username = "admin",
                       Password = "admin123"

                     }
                );
        } 
            
        public DbSet<Users> Users { get; set; }
        public DbSet<SchoolClass> Classes { get; set; }
            
        public DbSet<SubjectClass> Subjects { get; set; }
        public DbSet<Student> Students { get; set; }

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Grade> Grades { get; set; }

    }
}
