using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Models.Config
{
    public class BankTypeConfiguration : IEntityTypeConfiguration<Bank>
    {
        public void Configure(EntityTypeBuilder<Bank> builder)
        {
            ConfigureTable(builder);
            ConfigureAttributes(builder);
            ConfigureRelations(builder);
            AddSeed(builder);
        }

        private static void ConfigureTable(EntityTypeBuilder<Bank> builder)
        {
            builder.ToTable("banks");
            builder.HasKey(e => e.Id);
        }

        private static void ConfigureAttributes(EntityTypeBuilder<Bank> builder)
        {
            builder.Property(b => b.Id)
                .IsRequired()
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(b => b.Name)
                .IsRequired()
                .HasColumnName("name");

            builder.Property(b => b.AccountNumber)
                .IsRequired()
                .HasColumnName("account_number");
        }

        private static void ConfigureRelations(EntityTypeBuilder<Bank> builder)
        {
            builder.HasMany(b => b.BankTopUpRequests)
                .WithOne(r => r.Bank)
                .HasForeignKey(r => r.BankId);
        }


        private void AddSeed(EntityTypeBuilder<Bank> builder)
        {
            builder.HasData(new Bank
            {
                Id = 1,
                Name = "TESTBANK",
                AccountNumber = 999999,
            });
        }

    }
}
