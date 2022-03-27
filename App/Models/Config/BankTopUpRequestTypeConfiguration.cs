using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Models.Config
{
    public class BankTopUpRequestTypeConfiguration : IEntityTypeConfiguration<BankTopUpRequest>
    {
        private EntityTypeBuilder<BankTopUpRequest>? builder;

        public void Configure(EntityTypeBuilder<BankTopUpRequest> builder)
        {
            this.builder = builder;
            SetConfiguration();
        }

        private void SetConfiguration()
        {
            builder!.ToTable("bank_topup_request");
            builder!.HasKey(e => e.Id);
            builder!.HasIndex(e => e.Id).IsUnique();
            SetGeneralProperties();

            builder!.HasOne(r => r.History)
                .WithOne(h => h.BankRequest)
                .HasForeignKey<BankTopUpRequest>(r => r.Id);

            builder!.HasOne(r => r.From)
                .WithMany(u => u.BankTopUpRequests)
                .HasForeignKey(r => r.FromUserId);
        }

        private void SetGeneralProperties()
        {
            builder!.Property(b => b.Id)
                .IsRequired()
                .HasColumnName("bank_request_id");

            builder!.Property(b => b.VirtualAccountNumber)
                .IsRequired()
                .HasColumnName("virtual_account_number");

            builder!.Property(b => b.DueDate)
                .IsRequired()
                .HasColumnName("due_date")
                .HasConversion<string>();

            builder!.Property(b => b.Value)
                .IsRequired()
                .HasColumnName("value");

            builder!.Property(b => b.BankName)
                .IsRequired()
                .HasColumnName("bank_name");

            builder!.Property(b => b.FromUserId)
                .IsRequired()
                .HasColumnName("from_user_id");
        }
    }
}
