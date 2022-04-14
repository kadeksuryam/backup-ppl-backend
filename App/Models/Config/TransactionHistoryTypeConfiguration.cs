using App.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Models.Config
{
    public class TransactionHistoryTypeConfiguration
    {
        public void Configure(EntityTypeBuilder<TransactionHistory> builder)
        {
            ConfigureTable(builder);
            ConfigureAttributes(builder);
            ConfigureRelations(builder);
            AddSeed(builder);
        }

        private static void ConfigureTable(EntityTypeBuilder<TransactionHistory> builder)
        {
            builder.ToTable("transaction_histories");
            builder.HasKey(h => h.Id);
            builder.HasIndex(h => h.Id).IsUnique();
        }

        private static void ConfigureAttributes(EntityTypeBuilder<TransactionHistory> builder)
        {
            ConfigureGeneralAttributes(builder);
        }

        private static void ConfigureGeneralAttributes(EntityTypeBuilder<TransactionHistory> builder)
        {
            builder.Property(h => h.Id)
                .IsRequired()
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(h => h.CreatedAt)
                .IsRequired()
                .HasColumnName("created_at")
                .HasConversion<DateTimeModelConverter>();

            builder.Property(h => h.UpdatedAt)
                .IsRequired()
                .HasColumnName("updated_at")
                .HasConversion<DateTimeModelConverter>();

            builder.Property(h => h.FromUserId)
                .IsRequired()
                .HasColumnName("from_user_id");

            builder.Property(h => h.ToUserId)
                .IsRequired()
                .HasColumnName("to_user_id");

            builder.Property(h => h.Amount)
                .IsRequired()
                .HasColumnName("amount");

            builder.Property(r => r.Status)
                .IsRequired()
                .HasColumnName("status")
                .HasConversion<string>();
        }

        private static void ConfigureRelations(EntityTypeBuilder<TransactionHistory> builder)
        {
            ConfigureGeneralRelations(builder);
        }

        private static void ConfigureGeneralRelations(EntityTypeBuilder<TransactionHistory> builder)
        {
            builder.HasOne(h => h.From)
                .WithMany(u => u.TransactionHistoriesFrom)
                .HasForeignKey(h => h.FromUserId);

            builder.HasOne(h => h.To)
                .WithMany(u => u.TransactionHistoriesTo)
                .HasForeignKey(h => h.ToUserId);
        }


        private void AddSeed(EntityTypeBuilder<TransactionHistory> builder)
        {
            builder.HasData(new TransactionHistory
            {
                Id = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                FromUserId = 1,
                ToUserId = 2,
                Status = TransactionHistory.TransactionStatus.Success,
                Amount = 5000
            },
            new TransactionHistory
            {
                Id = 2,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                FromUserId = 2,
                ToUserId = 1,
                Status = TransactionHistory.TransactionStatus.Success,
                Amount = 5000
            });
        }
    }
}
