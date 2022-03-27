using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Models.Config
{
    public class TopUpHistoryTypeConfiguration : IEntityTypeConfiguration<TopUpHistory>
    {
        public void Configure(EntityTypeBuilder<TopUpHistory> builder)
        {
            ConfigureTable(builder);
            ConfigureAttributes(builder);
            ConfigureRelations(builder);
        }

        private static void ConfigureTable(EntityTypeBuilder<TopUpHistory> builder)
        {
            builder.ToTable("topup_histories");
            builder.HasKey(h => h.Id);
            builder.HasIndex(h => h.Id).IsUnique();
        }

        private static void ConfigureAttributes(EntityTypeBuilder<TopUpHistory> builder)
        {
            ConfigureGeneralAttributes(builder);
            ConfigureBankTopUpAttributes(builder);
            ConfigureVoucherTopUpAttributes(builder);
        }

        private static void ConfigureGeneralAttributes(EntityTypeBuilder<TopUpHistory> builder)
        {
            builder.Property(h => h.Id)
                .IsRequired()
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(h => h.CreatedAt)
                .IsRequired()
                .HasColumnName("created_at")
                .HasConversion<string>();

            builder.Property(h => h.UpdatedAt)
                .IsRequired()
                .HasColumnName("updated_at")
                .HasConversion<string>();

            builder.Property(h => h.FromUserId)
                .IsRequired()
                .HasColumnName("from_user_id");

            builder.Property(h => h.Amount)
                .IsRequired()
                .HasColumnName("amount");

            builder.Property(h => h.Method)
                .IsRequired()
                .HasColumnName("method")
                .HasConversion<string>();
        }

        private static void ConfigureBankTopUpAttributes(EntityTypeBuilder<TopUpHistory> builder)
        {
            builder.Property(b => b.BankRequestId)
                .IsRequired(false)
                .HasColumnName("bank_request_id");
        }

        private static void ConfigureVoucherTopUpAttributes(EntityTypeBuilder<TopUpHistory> builder)
        {
            // Add voucher id attribute here
        }

        private static void ConfigureRelations(EntityTypeBuilder<TopUpHistory> builder)
        {
            ConfigureGeneralRelations(builder);
            ConfigureBankTopUpRelations(builder);
            ConfigureVoucherTopUpRelations(builder);
        }

        private static void ConfigureGeneralRelations(EntityTypeBuilder<TopUpHistory> builder)
        {
            builder.HasOne(h => h.From)
                .WithMany(u => u.TopUpHistories)
                .HasForeignKey(h => h.FromUserId);
        }

        private static void ConfigureBankTopUpRelations(EntityTypeBuilder<TopUpHistory> builder)
        {
            builder.HasOne(h => h.BankRequest)
                .WithOne(r => r.History)
                .HasForeignKey<TopUpHistory>(h => h.BankRequestId);
        }

        private static void ConfigureVoucherTopUpRelations(EntityTypeBuilder<TopUpHistory> builder)
        {
            // Implement voucher connection here.
        }
    }
}
