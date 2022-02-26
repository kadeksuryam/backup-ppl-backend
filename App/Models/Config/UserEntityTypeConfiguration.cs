using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Models.Config
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(e => e.UserName);
            builder.HasIndex(e => e.UserName).IsUnique();

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
        }
    }
}
