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
    }
}
