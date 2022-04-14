using App.Helpers;
using App.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Models.Config
{
    public class BankTopUpRequestTypeConfiguration : IEntityTypeConfiguration<BankTopUpRequest>
    {
        public void Configure(EntityTypeBuilder<BankTopUpRequest> builder)
        {
            ConfigureTable(builder);
            ConfigureAttributes(builder);
            ConfigureRelations(builder);
            AddSeed(builder);
        }

        private static void ConfigureTable(EntityTypeBuilder<BankTopUpRequest> builder)
        {
            builder.ToTable("bank_topup_request");
            builder.HasKey(b => b.Id);
            builder.HasIndex(b => b.Id).IsUnique();
        }

        private static void ConfigureAttributes(EntityTypeBuilder<BankTopUpRequest> builder)
        {
            builder.Property(r => r.Id)
                .IsRequired()
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(r => r.CreatedAt)
                .IsRequired()
                .HasColumnName("created_at")
                .HasConversion<DateTimeModelConverter>();

            builder.Property(r => r.UpdatedAt)
                .IsRequired()
                .HasColumnName("updated_at")
                .HasConversion<DateTimeModelConverter>();

            builder.Property(r => r.ExpiredDate)
                .IsRequired()
                .HasColumnName("expired_date")
                .HasConversion<DateTimeModelConverter>();

            builder.Property(r => r.Amount)
                .IsRequired()
                .HasColumnName("amount");

            builder.Property(r => r.BankId)
                .IsRequired()
                .HasColumnName("bank_id");

            builder.Property(r => r.FromUserId)
                .IsRequired()
                .HasColumnName("from_user_id");

            builder.Property(r => r.Status)
                .IsRequired()
                .HasColumnName("status")
                .HasDefaultValue(RequestStatus.Pending)
                .HasConversion<string>();
        }

        private static void ConfigureRelations(EntityTypeBuilder<BankTopUpRequest> builder)
        {
            builder.HasOne(r => r.History)
                .WithOne(h => h.BankRequest)
                .HasForeignKey<TopUpHistory>(h => h.BankRequestId);

            builder.HasOne(r => r.From)
                .WithMany(u => u.BankTopUpRequests)
                .HasForeignKey(r => r.FromUserId);

            builder.HasOne(r => r.Bank)
                .WithMany(b => b.BankTopUpRequests)
                .HasForeignKey(r => r.BankId);
        }

        private void AddSeed(EntityTypeBuilder<BankTopUpRequest> builder)
        {
            builder.HasData(new BankTopUpRequest
            {
                Id = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ExpiredDate = DateTime.UtcNow.AddDays(1),
                FromUserId = 1,
                Amount = 50000,
                BankId = 1,
                Status = RequestStatus.Success,
            });
        }

    }
}
