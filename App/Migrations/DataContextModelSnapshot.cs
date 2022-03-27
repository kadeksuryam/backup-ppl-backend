﻿// <auto-generated />
using App.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace App.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("App.Models.Bank", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("AccountNumber")
                        .HasColumnType("bigint")
                        .HasColumnName("account_number");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("banks", (string)null);
                });

            modelBuilder.Entity("App.Models.BankTopUpRequest", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("Amount")
                        .HasColumnType("integer")
                        .HasColumnName("amount");

                    b.Property<long>("BankId")
                        .HasColumnType("bigint")
                        .HasColumnName("bank_id");

                    b.Property<string>("CreatedAt")
                        .IsRequired()
                        .HasColumnType("character varying(48)")
                        .HasColumnName("created_at");

                    b.Property<string>("ExpiredDate")
                        .IsRequired()
                        .HasColumnType("character varying(48)")
                        .HasColumnName("expired_date");

                    b.Property<long>("FromUserId")
                        .HasColumnType("bigint")
                        .HasColumnName("from_user_id");

                    b.Property<string>("Status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Pending")
                        .HasColumnName("status");

                    b.Property<long?>("ToBankId")
                        .HasColumnType("bigint");

                    b.Property<string>("UpdatedAt")
                        .IsRequired()
                        .HasColumnType("character varying(48)")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("BankId");

                    b.HasIndex("FromUserId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("bank_topup_request", (string)null);
                });

            modelBuilder.Entity("App.Models.Level", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<long>("RequiredExp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValue(0L)
                        .HasColumnName("required_exp");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("levels", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Name = "Bronze",
                            RequiredExp = 0L
                        },
                        new
                        {
                            Id = 2L,
                            Name = "Silver",
                            RequiredExp = 100L
                        },
                        new
                        {
                            Id = 3L,
                            Name = "Gold",
                            RequiredExp = 200L
                        },
                        new
                        {
                            Id = 4L,
                            Name = "Platinum",
                            RequiredExp = 300L
                        },
                        new
                        {
                            Id = 5L,
                            Name = "Diamond",
                            RequiredExp = 400L
                        },
                        new
                        {
                            Id = 6L,
                            Name = "Crazy Rich",
                            RequiredExp = 500L
                        });
                });

            modelBuilder.Entity("App.Models.TopUpHistory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("Amount")
                        .HasColumnType("integer")
                        .HasColumnName("amount");

                    b.Property<long?>("BankRequestId")
                        .HasColumnType("bigint")
                        .HasColumnName("bank_request_id");

                    b.Property<string>("CreatedAt")
                        .IsRequired()
                        .HasColumnType("character varying(48)")
                        .HasColumnName("created_at");

                    b.Property<long>("FromUserId")
                        .HasColumnType("bigint")
                        .HasColumnName("from_user_id");

                    b.Property<string>("Method")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("method");

                    b.Property<string>("UpdatedAt")
                        .IsRequired()
                        .HasColumnType("character varying(48)")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("BankRequestId")
                        .IsUnique();

                    b.HasIndex("FromUserId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("topup_histories", (string)null);
                });

            modelBuilder.Entity("App.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("Balance")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValue(0L)
                        .HasColumnName("balance");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("display_name");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("EncryptedPassword")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("encrypted_password");

                    b.Property<long>("Exp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValue(0L)
                        .HasColumnName("exp");

                    b.Property<long>("LevelId")
                        .HasColumnType("bigint")
                        .HasColumnName("levelId");

                    b.Property<string>("Type")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Standard")
                        .HasColumnName("login_type");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("LevelId");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("App.Models.BankTopUpRequest", b =>
                {
                    b.HasOne("App.Models.Bank", "Bank")
                        .WithMany("BankTopUpRequests")
                        .HasForeignKey("BankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("App.Models.User", "From")
                        .WithMany("BankTopUpRequests")
                        .HasForeignKey("FromUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bank");

                    b.Navigation("From");
                });

            modelBuilder.Entity("App.Models.TopUpHistory", b =>
                {
                    b.HasOne("App.Models.BankTopUpRequest", "BankRequest")
                        .WithOne("History")
                        .HasForeignKey("App.Models.TopUpHistory", "BankRequestId");

                    b.HasOne("App.Models.User", "From")
                        .WithMany("TopUpHistories")
                        .HasForeignKey("FromUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BankRequest");

                    b.Navigation("From");
                });

            modelBuilder.Entity("App.Models.User", b =>
                {
                    b.HasOne("App.Models.Level", "Level")
                        .WithMany("Users")
                        .HasForeignKey("LevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Level");
                });

            modelBuilder.Entity("App.Models.Bank", b =>
                {
                    b.Navigation("BankTopUpRequests");
                });

            modelBuilder.Entity("App.Models.BankTopUpRequest", b =>
                {
                    b.Navigation("History");
                });

            modelBuilder.Entity("App.Models.Level", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("App.Models.User", b =>
                {
                    b.Navigation("BankTopUpRequests");

                    b.Navigation("TopUpHistories");
                });
#pragma warning restore 612, 618
        }
    }
}
