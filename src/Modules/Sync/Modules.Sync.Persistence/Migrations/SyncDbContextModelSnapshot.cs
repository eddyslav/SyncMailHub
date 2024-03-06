﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Modules.Sync.Persistence;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Modules.Sync.Persistence.Migrations
{
    [DbContext(typeof(SyncDbContext))]
    partial class SyncDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("sync")
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Modules.Sync.Domain.ServiceAccountSyncStates.ServiceAccountSyncState", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("ServiceAccountSyncStates", "sync");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Modules.Sync.Domain.ServiceAccounts.ServiceAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("HubId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("HubId")
                        .IsUnique();

                    b.ToTable("ServiceAccounts", "sync");
                });

            modelBuilder.Entity("Persistence.Inbox.InboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("json");

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("OccuredAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("ProcessedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("InboxMessages", "sync");
                });

            modelBuilder.Entity("Persistence.Inbox.InboxMessageConsumer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id", "Name");

                    b.ToTable("InboxMessagesConsumers", "sync");
                });

            modelBuilder.Entity("Persistence.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("json");

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("OccuredAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("ProcessedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessages", "sync");
                });

            modelBuilder.Entity("Persistence.Outbox.OutboxMessageConsumer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id", "Name");

                    b.ToTable("OutboxMessagesConsumers", "sync");
                });

            modelBuilder.Entity("Modules.Sync.Domain.ServiceAccountSyncStates.GoogleServiceAccountSyncState", b =>
                {
                    b.HasBaseType("Modules.Sync.Domain.ServiceAccountSyncStates.ServiceAccountSyncState");

                    b.Property<decimal>("HistoryId")
                        .HasColumnType("numeric(20,0)");

                    b.ToTable("GoogleServiceAccountSyncStates", "sync");
                });

            modelBuilder.Entity("Modules.Sync.Domain.ServiceAccountSyncStates.ServiceAccountSyncState", b =>
                {
                    b.HasOne("Modules.Sync.Domain.ServiceAccounts.ServiceAccount", null)
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Modules.Sync.Domain.ServiceAccountSyncStates.GoogleServiceAccountSyncState", b =>
                {
                    b.HasOne("Modules.Sync.Domain.ServiceAccountSyncStates.ServiceAccountSyncState", null)
                        .WithOne()
                        .HasForeignKey("Modules.Sync.Domain.ServiceAccountSyncStates.GoogleServiceAccountSyncState", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
