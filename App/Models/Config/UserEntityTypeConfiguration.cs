using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Models.Config
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(e => e.Id);

            // Uniqueness configuration
            builder.HasIndex(e => e.Id).IsUnique();
            builder.HasIndex(e => e.UserName).IsUnique();
            builder.HasIndex(e => e.Email).IsUnique();

            builder.Property(b => b.Id)
                .IsRequired()
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(b => b.UserName)
                .IsRequired()
                .HasColumnName("username");

            builder.Property(b => b.EncryptedPassword)
                .IsRequired()
                .HasColumnName("encrypted_password");

            builder.Property(b => b.DisplayName)
                .IsRequired()
                .HasColumnName("display_name");

            builder.Property(b => b.Email)
                .IsRequired()
                .HasColumnName("email");

            builder.Property(b => b.Balance)
                .IsRequired()
                .HasColumnName("balance")
                .HasDefaultValue(0);

            builder.Property(b => b.Exp)
                .IsRequired()
                .HasColumnName("exp")
                .HasDefaultValue(0);

            builder.Property(b => b.LevelId)
                .IsRequired()
                .HasColumnName("levelId");

            builder.HasOne(u => u.Level)
                .WithMany(l => l.Users)
                .HasForeignKey(u => u.LevelId);

            builder.Property(b => b.Type)
               .IsRequired()
               .HasColumnName("login_type")
               .HasDefaultValue(User.LoginType.Standard)
               .HasConversion<string>();

            builder.HasMany(u => u.TopUpHistories)
                .WithOne(h => h.From)
                .HasForeignKey(h => h.FromUserId);

            builder.HasMany(u => u.BankTopUpRequests)
                .WithOne(r => r.From)
                .HasForeignKey(r => r.FromUserId);
        }
    }
}
