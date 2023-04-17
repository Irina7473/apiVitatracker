using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace MedicineReminderAPI.Models
{
    public class AppApiContext : DbContext
    {
        
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<NotificationSetting> NotificationSettings { get; set; } = null!;
        public DbSet<Remedy> Remedys { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Usage> Usages { get; set; } = null!;
        public DbSet<HistoryRemedy> HistoryRemedys { get; set; } = null!;


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
            //установка значений по умолчанию
            modelBuilder.Entity<User>().Property(u => u.NotUsed).HasDefaultValue(false);
            modelBuilder.Entity<User>().Property(u => u.Created).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            modelBuilder.Entity<User>().Property(u => u.Updated).HasDefaultValueSql("CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6)");


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

            // заполнение таблиц данными
            /* 
             modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Bob", Email = "bob@mail.ru", Password = BCrypt.Net.BCrypt.HashPassword("12345678"), NotUsed = false, Created = DateTime.Now, Updated = DateTime.Now },
                new User { Id = 2, Name = "Eva", Email = "Eva@mail.ru", Password = BCrypt.Net.BCrypt.HashPassword("12345678"), NotUsed = false, Created = DateTime.Now, Updated = DateTime.Now }
            );

             modelBuilder.Entity<NotificationSetting>().HasData(
                new NotificationSetting { Id = 1, UserId = 1 },
                new NotificationSetting { Id = 2, UserId = 2 }
            );

             modelBuilder.Entity<Remedy>().HasData(
                 new Remedy { Id = 1, UserId = 1, Name = "витамин", Type = 1, NotUsed = false, Created = DateTime.Now, Updated = DateTime.Now, Courses = {} },
                 new Remedy { Id = 2, UserId = 2, Name = "аспирин", Type = 1, NotUsed = false, Created = DateTime.Now, Updated = DateTime.Now, Courses = {} }
              );

             modelBuilder.Entity<Course>().HasData(
                 new Course { Id = 1, RemedyId = 1, Regime = 1, StartDate = 123, NotUsed = false, Created = DateTime.Now, Updated = DateTime.Now, Usages = {} },
                 new Course { Id = 2, RemedyId = 2, Regime = 1, StartDate = 123, NotUsed = false, Created = DateTime.Now, Updated = DateTime.Now, Usages = {} }
             );
             */
        }


    }
}