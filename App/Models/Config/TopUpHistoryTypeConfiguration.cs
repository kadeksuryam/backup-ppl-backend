using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Models.Config
{
    public class TopUpHistoryTypeConfiguration : IEntityTypeConfiguration<TopUpHistory>
    {
        private EntityTypeBuilder<TopUpHistory>? builder;

        public void Configure(EntityTypeBuilder<TopUpHistory> builder)
        {
            this.builder = builder;
            SetGeneralConfiguration();
            SetBankTopUpConfiguration();
            SetVoucherTopUpConfiguration();
        }

        private void SetGeneralConfiguration()
        {
            builder!.ToTable("topup_histories");
            builder!.HasKey(e => e.Id);
            builder!.HasIndex(e => e.Id).IsUnique();
            SetGeneralProperties();

            builder!.HasOne(h => h.From)
                .WithMany(u => u.TopUpHistories)
                .HasForeignKey(h => h.FromUserId);
        }

        private void SetGeneralProperties()
        {
            builder!.Property(b => b.Id)
                .IsRequired()
                .HasColumnName("topup_id");

            builder!.Property(b => b.CreatedAt)
                .IsRequired()
                .HasColumnName("created_at")
                .HasConversion<string>();

            builder!.Property(b => b.UpdatedAt)
                .IsRequired()
                .HasColumnName("updated_at")
                .HasConversion<string>();

            builder!.Property(b => b.FromUserId)
                .IsRequired()
                .HasColumnName("from_user_id");

            builder!.Property(b => b.Value)
                .IsRequired()
                .HasColumnName("value");

            builder!.Property(b => b.CurrentStatus)
                .IsRequired()
                .HasColumnName("current_status")
                .HasDefaultValue(Status.Pending)
                .HasConversion<string>();

            builder!.Property(b => b.Method)
                .IsRequired()
                .HasColumnName("method")
                .HasConversion<string>();
        }

        private void SetBankTopUpConfiguration()
        {
            builder!.Property(b => b.BankRequestId)
                .HasColumnName("bank_request_id");

            builder!.HasOne(h => h.BankRequest)
                .WithOne(r => r.History)
                .HasForeignKey<TopUpHistory>(h => h.BankRequestId);
        }

        private void SetVoucherTopUpConfiguration()
        {
            // builder!.Property(b => b.UsedVoucherId)
            //     .HasColumnName("used_voucher_id");

            // Implement voucher connection here.
        }
    }
}
