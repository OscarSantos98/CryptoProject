using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CryptoAPI.Models
{
    public partial class CryptoDBContext : DbContext
    {
        public CryptoDBContext()
        {
        }

        public CryptoDBContext(DbContextOptions<CryptoDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Crypto> Cryptos { get; set; } = null!;
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Theme> Themes { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserAction> UserActions { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=CryptoDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Crypto>(entity =>
            {
                entity.ToTable("crypto");

                entity.Property(e => e.CryptoId).HasColumnName("crypto_id");

                entity.Property(e => e.CoupleName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("couple_name");

                entity.Property(e => e.LastUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_update")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Value).HasColumnName("value");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Cryptos)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__crypto__user_id__38996AB5");
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.TokenId)
                    .HasName("PK__refreshT__CB3C9E17640E8ED1");

                entity.ToTable("refreshToken");

                entity.Property(e => e.TokenId).HasColumnName("token_id");

                entity.Property(e => e.ExpiryDate)
                    .HasColumnType("datetime")
                    .HasColumnName("expiry_date");

                entity.Property(e => e.Token)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("token");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RefreshTokens)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__refreshTo__user___398D8EEE");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.RoleDesc)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("role_desc")
                    .HasDefaultValueSql("('User')");
            });

            modelBuilder.Entity<Theme>(entity =>
            {
                entity.ToTable("themes");

                entity.HasIndex(e => e.ThemeName, "UQ__themes__3D686635035C733D")
                    .IsUnique();

                entity.Property(e => e.ThemeId).HasColumnName("theme_id");

                entity.Property(e => e.ThemeName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("theme_name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.Email, "UQ__user__AB6E6164D397E195")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.ReceiveNotifications)
                    .HasColumnName("receiveNotifications")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RoleId)
                    .HasColumnName("role_id")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ThemeId)
                    .HasColumnName("theme_id")
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__user__role_id__37A5467C");

                entity.HasOne(d => d.Theme)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.ThemeId)
                    .HasConstraintName("FK__user__theme_id__36B12243");
            });

            modelBuilder.Entity<UserAction>(entity =>
            {
                entity.HasKey(e => e.UserActionsId)
                    .HasName("PK__user_act__1C1FCF854834CE7D");

                entity.ToTable("user_actions");

                entity.Property(e => e.UserActionsId).HasColumnName("user_actions_id");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.UserActions)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("user_actions");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserActions)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__user_acti__role___35BCFE0A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
