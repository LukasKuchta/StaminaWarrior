﻿// <auto-generated />
using System;
using Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StaminaWarrior.Server.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("StaminaWarrior.Server.Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<uint>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.Property<int>("WarriorsCount")
                        .HasColumnType("integer")
                        .HasColumnName("warriors_count");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("StaminaWarrior.Server.Domain.WarriorPaths.WarriorPath", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime?>("End")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("end");

                    b.Property<DateTime>("Start")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start");

                    b.Property<Guid>("WarriorId")
                        .HasColumnType("uuid")
                        .HasColumnName("warrior_id");

                    b.HasKey("Id")
                        .HasName("pk_warrior_path");

                    b.ToTable("warrior_path", (string)null);
                });

            modelBuilder.Entity("StaminaWarrior.Server.Domain.Warriors.Warrior", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<uint>("CurrentLevel")
                        .HasColumnType("bigint")
                        .HasColumnName("current_level");

                    b.Property<uint>("Experience")
                        .HasColumnType("bigint")
                        .HasColumnName("experience");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_warriors");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_warriors_user_id");

                    b.ToTable("warriors", (string)null);
                });

            modelBuilder.Entity("StaminaWarrior.Server.Infrastructure.Messaging.InternalCommands.InternalCommand", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("InternalCommandId")
                        .HasColumnType("uuid")
                        .HasColumnName("internal_command_id");

                    b.Property<string>("JsonContent")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("json_content");

                    b.Property<DateTime?>("ProcessedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("processed_date");

                    b.Property<DateTime>("ScheduledDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("scheduled_date");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_internal_commands");

                    b.ToTable("internal_commands", (string)null);
                });

            modelBuilder.Entity("StaminaWarrior.Server.Infrastructure.Messaging.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("Attempt")
                        .HasColumnType("integer")
                        .HasColumnName("attempt");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("json")
                        .HasColumnName("content");

                    b.Property<DateTime>("OccurredOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("occurred_on_utc");

                    b.Property<DateTime?>("ProcessedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("processed_on_utc");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_outbox_messages");

                    b.ToTable("outbox_messages", (string)null);
                });

            modelBuilder.Entity("StaminaWarrior.Server.Infrastructure.Messaging.Outbox.OutboxMessageError", b =>
                {
                    b.Property<Guid>("OutboxMessageId")
                        .HasColumnType("uuid")
                        .HasColumnName("outbox_message_id");

                    b.Property<string>("HandlerName")
                        .HasColumnType("text")
                        .HasColumnName("handler_name");

                    b.Property<string>("Error")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("error");

                    b.Property<string>("PublicEventName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("public_event_name");

                    b.HasKey("OutboxMessageId", "HandlerName")
                        .HasName("pk_outbox_message_errors");

                    b.ToTable("outbox_message_errors", (string)null);
                });

            modelBuilder.Entity("StaminaWarrior.Server.Infrastructure.Messaging.PublicEvents.IdempontcyPublicEvent", b =>
                {
                    b.Property<Guid>("PublicEventId")
                        .HasColumnType("uuid")
                        .HasColumnName("public_event_id");

                    b.Property<string>("HandlerName")
                        .HasColumnType("text")
                        .HasColumnName("handler_name");

                    b.Property<DateTime>("ProcessedDateUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("processed_date_utc");

                    b.HasKey("PublicEventId", "HandlerName")
                        .HasName("pk_idempotency_public_events");

                    b.ToTable("idempotency_public_events", (string)null);
                });

            modelBuilder.Entity("StaminaWarrior.Server.Domain.Warriors.Warrior", b =>
                {
                    b.HasOne("StaminaWarrior.Server.Domain.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_warriors_users_user_id");
                });

            modelBuilder.Entity("StaminaWarrior.Server.Infrastructure.Messaging.Outbox.OutboxMessageError", b =>
                {
                    b.HasOne("StaminaWarrior.Server.Infrastructure.Messaging.Outbox.OutboxMessage", null)
                        .WithMany("Errors")
                        .HasForeignKey("OutboxMessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_outbox_message_errors_outbox_message_outbox_message_id");
                });

            modelBuilder.Entity("StaminaWarrior.Server.Infrastructure.Messaging.Outbox.OutboxMessage", b =>
                {
                    b.Navigation("Errors");
                });
#pragma warning restore 612, 618
        }
    }
}
