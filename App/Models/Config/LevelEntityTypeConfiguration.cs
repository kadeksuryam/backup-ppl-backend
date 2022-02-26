using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Models.Config
{
    public class LevelEntityTypeConfiguration : IEntityTypeConfiguration<Level>
    {
        public void Configure(EntityTypeBuilder<Level> builder)
        {
            builder.ToTable("levels");
            builder.HasKey(b => b.Id);
            builder.HasIndex(b => b.Id).IsUnique();

            builder.Property(b => b.Id)
                .IsRequired()
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(b => b.Name)
                .IsRequired()
                .HasColumnName("name");

            builder.Property(b => b.RequiredExp)
                .IsRequired()
                .HasColumnName("required_exp")
                .HasDefaultValue(0);

            builder.HasMany(l => l.Users)
                .WithOne(u => u.Level)
                .HasForeignKey(l => l.LevelId);

            AddSeed(builder);
        }

        public void AddSeed(EntityTypeBuilder<Level> builder)
        {
            builder.HasData(new Level
            {
                Id = 1,
                Name = "Bronze",
                RequiredExp = 0,
            }, new Level
            {
                Id = 2,
                Name = "Silver",
                RequiredExp = 100,
            }, new Level { 
                Id = 3,
                Name = "Gold",
                RequiredExp = 200,
            }, new Level {
                Id = 4,
                Name = "Platinum",
                RequiredExp = 300,
            }, new Level { 
                Id = 5,
                Name = "Diamond",
                RequiredExp = 400,
            }, new Level {
                Id = 6,
                Name = "Crazy Rich",
                RequiredExp = 500,
            });
        }
    }
}
