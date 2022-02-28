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

            builder.Property(b => b.Level)
                .IsRequired()
                .HasColumnName("level")
                .HasDefaultValue(1); // New user starts from level 1

            builder.Property(b => b.Type)
               .IsRequired()
               .HasColumnName("login_type")
               .HasDefaultValue(User.LoginType.Standard)
               .HasConversion<string>();
        }
    }
}
