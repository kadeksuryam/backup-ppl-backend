using App.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Models.Config
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        private readonly IBcryptWrapper _bcryptWrapper = new BcryptWrapper();

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
            builder.Property(b => b.Role)
                .IsRequired()
                .HasColumnName("user_role")
                .HasDefaultValue(User.UserRole.Customer)
                .HasConversion<string>();

            builder.HasMany(u => u.TopUpHistories)
                .WithOne(h => h.From)
                .HasForeignKey(h => h.FromUserId);

            builder.HasMany(u => u.BankTopUpRequests)
                .WithOne(r => r.From)
                .HasForeignKey(r => r.FromUserId);

            AddSeed(builder);
        }

        private void AddSeed(EntityTypeBuilder<User> builder)
        {
            builder.HasData(new User
            {
                Id = 1,
                UserName = "cakrawalaid",
                EncryptedPassword = _bcryptWrapper.hashPassword("ppl2022"),
                DisplayName = "Admin",
                Email = "admin@cakrawala.id",
                Balance = 0,
                Exp = 0,
                LevelId = 1,
                Type = User.LoginType.Standard,
                Role = User.UserRole.Admin,
            },
            new User
            {
                Id = 2,
                UserName = "tes1",
                EncryptedPassword = _bcryptWrapper.hashPassword("tes1"),
                DisplayName = "tes1",
                Email = "tes1@cakrawala.id",
                Balance = 0,
                Exp = 0,
                LevelId = 1,
                Type = User.LoginType.Standard,
                Role = User.UserRole.Customer,
            });
        }
    }
}
