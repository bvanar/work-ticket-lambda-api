using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace well_project_api.Models
{
    public class WellDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Job> Job { get; set; }
        public DbSet<JobTasks> JobTasks { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<UserPermissions> UserPermissions { get; set; }
        public DbSet<UserJob> UserJob { get; set; }
        public DbSet<UserCompany> UserCompany { get; set; }


        public WellDbContext(DbContextOptions<WellDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Company>().ToTable("Company");
            modelBuilder.Entity<Job>().ToTable("Job");
            modelBuilder.Entity<JobTasks>().ToTable("JobTasks");
            modelBuilder.Entity<Permissions>().ToTable("Permissions");
            modelBuilder.Entity<UserPermissions>().ToTable("UserPermissions");
            modelBuilder.Entity<UserJob>().ToTable("UserJob");
            modelBuilder.Entity<UserCompany>().ToTable("UserCompany");

            // Primary Keys
            modelBuilder.Entity<User>().HasKey(u => u.UserId).HasName("UserId");
            modelBuilder.Entity<Company>().HasKey(u => u.CompanyId).HasName("CompanyId");
            modelBuilder.Entity<Job>().HasKey(u => u.JobId).HasName("JobId");
            modelBuilder.Entity<JobTasks>().HasKey(u => u.TaskId).HasName("TaskId");
            modelBuilder.Entity<Permissions>().HasKey(u => u.PermissionId).HasName("PermissionId");
            modelBuilder.Entity<UserPermissions>().HasKey(u => u.UserPermissionId).HasName("UserPermissionId");
            modelBuilder.Entity<UserJob>().HasKey(u => u.UserJobId).HasName("UserJobId");
            modelBuilder.Entity<UserCompany>().HasKey(u => u.UserCompanyId).HasName("UserCompanyId");

            // User Columns
            modelBuilder.Entity<User>().Property(u => u.UserId).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<User>().Property(u => u.UserName).HasColumnType("varchar(50)").IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Password).HasColumnType("varchar(100)").IsRequired();           
            modelBuilder.Entity<User>().Property(u => u.LastLogin).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<User>().Property(u => u.IsAdmin).HasColumnType("bit").IsRequired();
            modelBuilder.Entity<User>().Property(u => u.IsDeleted).HasColumnType("bit").IsRequired();

            // Company Columns
            modelBuilder.Entity<Company>().Property(u => u.CompanyId).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<Company>().Property(u => u.CompanyName).HasColumnType("varchar(50)").IsRequired();
            modelBuilder.Entity<Company>().Property(u => u.IsDeleted).HasColumnType("bit").IsRequired();
            modelBuilder.Entity<Company>().Property(u => u.OwnerId).HasColumnType("int").IsRequired();

            // Job Columns
            modelBuilder.Entity<Job>().Property(u => u.JobId).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<Job>().Property(u => u.JobName).HasColumnType("varchar(50)").IsRequired();
            modelBuilder.Entity<Job>().Property(u => u.CompanyId).HasColumnType("int").IsRequired();
            modelBuilder.Entity<Job>().Property(u => u.IsDeleted).HasColumnType("bit").IsRequired();
            modelBuilder.Entity<Job>().Property(u => u.OwnerId).HasColumnType("int").IsRequired();

            // JobTasks Columns
            modelBuilder.Entity<JobTasks>().Property(u => u.TaskId).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();            
            modelBuilder.Entity<JobTasks>().Property(u => u.JobId).HasColumnType("int").IsRequired();
            modelBuilder.Entity<JobTasks>().Property(u => u.TaskName).HasColumnType("varchar(200)").IsRequired();
            modelBuilder.Entity<JobTasks>().Property(u => u.TaskOrder).HasColumnType("int").IsRequired();
            modelBuilder.Entity<JobTasks>().Property(u => u.Completed).HasColumnType("bit").IsRequired();
            modelBuilder.Entity<JobTasks>().Property(u => u.CompletedDate).HasColumnType("datetime");
            modelBuilder.Entity<JobTasks>().Property(u => u.IsDeleted).HasColumnType("bit").IsRequired();            

            // Permissions Columns
            modelBuilder.Entity<Permissions>().Property(u => u.PermissionId).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<Permissions>().Property(u => u.PermissionName).HasColumnType("varchar(50)").IsRequired();

            // UserPermissions Columns
            modelBuilder.Entity<UserPermissions>().Property(u => u.UserPermissionId).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<UserPermissions>().Property(u => u.UserId).HasColumnType("int").IsRequired();
            modelBuilder.Entity<UserPermissions>().Property(u => u.PermissionId).HasColumnType("int").IsRequired();

            // UserJob Columns
            modelBuilder.Entity<UserJob>().Property(u => u.UserJobId).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<UserJob>().Property(u => u.JobId).HasColumnType("int").IsRequired();
            modelBuilder.Entity<UserJob>().Property(u => u.UserId).HasColumnType("int").IsRequired();
        }
    }
}
