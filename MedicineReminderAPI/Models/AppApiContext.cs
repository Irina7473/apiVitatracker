using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace MedicineReminderAPI.Models
{    
    public class AppApiContext : IdentityUserContext<User, int> 
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
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(b =>
            {
                b.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                b.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                b.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

            });

            modelBuilder.Entity<User>(b =>
            {
                b.ToTable("Users");
                b.Property(u => u.UserName).HasColumnName("Name").HasMaxLength(120);
                b.Property(u => u.Email).HasMaxLength(64);
                b.Property(u => u.PasswordHash).HasColumnName("Password").HasMaxLength(120);
                b.Property(u => u.EmailConfirmed).HasDefaultValue(false);
                b.Property(u => u.PhoneNumberConfirmed).HasDefaultValue(false);
                b.Property(u => u.TwoFactorEnabled).HasDefaultValue(false);
                b.Property(u => u.LockoutEnabled).HasDefaultValue(false);
                b.Property(u => u.AccessFailedCount).HasDefaultValue(5);
                b.Property(u => u.NotUsed).HasDefaultValue(false);
                b.Property(u => u.Created).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
                b.Property(u => u.Updated).HasDefaultValueSql("CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6)");
            });

            modelBuilder.Entity<IdentityUserClaim<int>>(b =>
            {
                b.ToTable("UserClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<int>>(b =>
            {
                b.ToTable("UserLogins");
            });

            modelBuilder.Entity<IdentityUserToken<int>>(b =>
            {
                b.ToTable("UserTokens");
            });

            


            //установка значений по умолчанию
            /*
            modelBuilder.Entity<User>().Property(u => u.NotUsed).HasDefaultValue(false);
            modelBuilder.Entity<User>().Property(u => u.Created).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            modelBuilder.Entity<User>().Property(u => u.Updated).HasDefaultValueSql("CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6)");
            */

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