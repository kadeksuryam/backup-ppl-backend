using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Models.Config
{
    public class VoucherEntityTypeConfiguration : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.ToTable("vouchers");
            builder.HasKey(b => b.Id);
            builder.HasIndex(b => b.Code).IsUnique();

            builder.Property(b => b.Id)
                .IsRequired()
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(b => b.Code)
                .IsRequired()
                .HasColumnName("code");

            builder.Property(b => b.Amount)
                .IsRequired()
                .HasColumnName("amount");

            builder.Property(b => b.CreatedAt)
                .IsRequired()
                .HasColumnName("created_at")
                .HasConversion<string>();

            builder.Property(b => b.UpdatedAt)
                .IsRequired()
                .HasColumnName("updated_at")
                .HasConversion<string>();

            builder.Property(b => b.IsUsed)
                .IsRequired()
                .HasColumnName("is_used")
                .HasDefaultValue(false);

            ConfigureRelations(builder);
            AddSeed(builder);
        }

        private static void ConfigureRelations(EntityTypeBuilder<Voucher> builder)
        {
            builder.HasOne(v => v.History)
                .WithOne(h => h.Voucher)
                .HasForeignKey<TopUpHistory>(h => h.VoucherId);
        }

        private void AddSeed(EntityTypeBuilder<Voucher> builder)
        {
            builder.HasData(new Voucher
            {
                Id = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Code = "ZZZZZZ",
                Amount = 50000,
                IsUsed = false,
            });
        }
    }
}
