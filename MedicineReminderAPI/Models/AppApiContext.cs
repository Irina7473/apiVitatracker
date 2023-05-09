using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace MedicineReminderAPI.Models
{
    public class AppApiContext : DbContext
    {
        
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<NotificationSetting> NotificationSettings { get; set; } = null!;
        public DbSet<Remedy> Remedies { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Usage> Usages { get; set; } = null!;
        public DbSet<HistoryRemedy> HistoryRemedies { get; set; } = null!;


        public AppApiContext(DbContextOptions<AppApiContext> options) : base(options)       
        {
            //Database.EnsureCreated();            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           // optionsBuilder.UseLazyLoadingProxies();        // подключение lazy loading
        }

        // Устанавливаю значения по умолчанию
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<User>(u =>
            {
                u.HasIndex(u => u.Email).IsUnique();
                u.Property(u => u.NotUsed).HasDefaultValue(false);
                u.Property(u => u.Created).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
                u.Property(u => u.Updated).HasDefaultValueSql("CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6)");
            });


            modelBuilder.Entity<NotificationSetting>().Property(u => u.IsEnabled).HasDefaultValue(true);
            modelBuilder.Entity<NotificationSetting>().Property(u => u.IsFloat).HasDefaultValue(false);
            modelBuilder.Entity<NotificationSetting>().Property(u => u.MedicalControl).HasDefaultValue(false);
            modelBuilder.Entity<NotificationSetting>().Property(u => u.NextCourseStart).HasDefaultValue(true);

            modelBuilder.Entity<Remedy>().Property(u => u.Icon).HasDefaultValue(0);
            modelBuilder.Entity<Remedy>().Property(u => u.Color).HasDefaultValue(0);
            modelBuilder.Entity<Remedy>().Property(u => u.NotUsed).HasDefaultValue(false);
            modelBuilder.Entity<Remedy>().Property(u => u.Created).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            modelBuilder.Entity<Remedy>().Property(u => u.Updated).HasDefaultValueSql("CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6)");

            modelBuilder.Entity<Course>().Property(u => u.NotUsed).HasDefaultValue(false);
            modelBuilder.Entity<Course>().Property(u => u.IsFinished).HasDefaultValue(false);
            modelBuilder.Entity<Course>().Property(u => u.IsInfinite).HasDefaultValue(false);
            modelBuilder.Entity<Course>().Property(u => u.Created).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            modelBuilder.Entity<Course>().Property(u => u.Updated).HasDefaultValueSql("CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6)");

            modelBuilder.Entity<Usage>().Property(u => u.NotUsed).HasDefaultValue(false);
            modelBuilder.Entity<Usage>().Property(u => u.Created).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            modelBuilder.Entity<Usage>().Property(u => u.Updated).HasDefaultValueSql("CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6)");

            modelBuilder.Entity<HistoryRemedy>().Property(u => u.Created).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

           
        }


    }
}