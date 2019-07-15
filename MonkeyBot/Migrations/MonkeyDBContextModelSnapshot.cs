﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MonkeyBot.Database;

namespace MonkeyBot.Migrations
{
    [DbContext(typeof(MonkeyDBContext))]
    partial class MonkeyDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("MonkeyBot.Database.Entities.AnnouncementEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<ulong>("ChannelId");

                    b.Property<string>("CronExpression");

                    b.Property<DateTime?>("ExecutionTime");

                    b.Property<ulong>("GuildId");

                    b.Property<string>("Message")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("Announcements");
                });

            modelBuilder.Entity("MonkeyBot.Database.Entities.FeedEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<ulong>("ChannelId");

                    b.Property<ulong>("GuildId");

                    b.Property<DateTime?>("LastUpdate");

                    b.Property<string>("URL")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Feeds");
                });

            modelBuilder.Entity("MonkeyBot.Database.Entities.GameServerEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<ulong>("ChannelId");

                    b.Property<string>("GameServerType")
                        .IsRequired();

                    b.Property<string>("GameVersion");

                    b.Property<ulong>("GuildId");

                    b.Property<string>("IPAsString")
                        .IsRequired()
                        .HasColumnName("IP");

                    b.Property<DateTime?>("LastVersionUpdate");

                    b.Property<ulong?>("MessageId");

                    b.HasKey("Id");

                    b.ToTable("GameServers");
                });

            modelBuilder.Entity("MonkeyBot.Database.Entities.GameSubscriptionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("GameName")
                        .IsRequired();

                    b.Property<ulong>("GuildId");

                    b.Property<ulong>("UserId");

                    b.HasKey("Id");

                    b.ToTable("GameSubscriptions");
                });

            modelBuilder.Entity("MonkeyBot.Database.Entities.RoleButtonLinkEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EmoteString")
                        .IsRequired();

                    b.Property<ulong>("GuildId");

                    b.Property<ulong>("MessageId");

                    b.Property<ulong>("RoleId");

                    b.HasKey("Id");

                    b.ToTable("RoleButtonLinks");
                });

            modelBuilder.Entity("MonkeyBot.Database.Entities.TriviaScoreEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<ulong>("GuildId");

                    b.Property<int>("Score");

                    b.Property<ulong>("UserId");

                    b.HasKey("Id");

                    b.ToTable("TriviaScores");
                });

            modelBuilder.Entity("MonkeyBot.Models.BenzenFact", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Fact")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("BenzenFacts");
                });

            modelBuilder.Entity("MonkeyBot.Models.GuildConfig", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CommandPrefix")
                        .IsRequired();

                    b.Property<ulong>("GoodbyeMessageChannelId");

                    b.Property<string>("GoodbyeMessageText");

                    b.Property<ulong>("GuildID");

                    b.Property<string>("Rules");

                    b.Property<ulong>("WelcomeMessageChannelId");

                    b.Property<string>("WelcomeMessageText");

                    b.HasKey("ID");

                    b.ToTable("GuildConfigs");
                });
#pragma warning restore 612, 618
        }
    }
}
