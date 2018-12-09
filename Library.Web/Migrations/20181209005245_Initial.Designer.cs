﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Library.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Library.Web.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20181209005245_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Library.Database.Models.Author", b =>
                {
                    b.Property<Guid>("AuthorGuid")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Note");

                    b.Property<string>("Patronymic")
                        .HasMaxLength(100);

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("AuthorGuid");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("Library.Database.Models.Book", b =>
                {
                    b.Property<Guid>("BookGuid")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Annotation");

                    b.Property<List<Guid>>("AuthorGuid");

                    b.Property<Guid?>("AuthorGuid1");

                    b.Property<string>("BuyUri");

                    b.Property<string>("Cost");

                    b.Property<string>("Cover");

                    b.Property<DateTimeOffset>("CreationDateTimeOffset");

                    b.Property<string>("Format");

                    b.Property<string>("ImageUri");

                    b.Property<int>("NumberOfPages");

                    b.Property<Guid>("PublishingGuid");

                    b.Property<Guid>("TechnologyGuid");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<int>("Year");

                    b.HasKey("BookGuid");

                    b.HasIndex("AuthorGuid1");

                    b.HasIndex("PublishingGuid");

                    b.HasIndex("TechnologyGuid");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("Library.Database.Models.Publishing", b =>
                {
                    b.Property<Guid>("PublishingGuid")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("House");

                    b.Property<string>("Name");

                    b.Property<long>("Postcode");

                    b.Property<string>("State");

                    b.Property<string>("Street");

                    b.HasKey("PublishingGuid");

                    b.ToTable("Publishings");
                });

            modelBuilder.Entity("Library.Database.Models.RSS.RssItem", b =>
                {
                    b.Property<Guid>("RssItemGuid")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Enclosure");

                    b.Property<string>("Link");

                    b.Property<DateTimeOffset>("PubDate");

                    b.Property<Guid>("RssSourceGuid");

                    b.Property<string>("Title");

                    b.HasKey("RssItemGuid");

                    b.HasIndex("RssSourceGuid");

                    b.ToTable("RssItems");
                });

            modelBuilder.Entity("Library.Database.Models.RSS.RssSource", b =>
                {
                    b.Property<Guid>("RssSourceGuid")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Title");

                    b.Property<string>("Uri");

                    b.HasKey("RssSourceGuid");

                    b.ToTable("RssSources");
                });

            modelBuilder.Entity("Library.Database.Models.Technology", b =>
                {
                    b.Property<Guid>("TechnologyGuid")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Language");

                    b.Property<string>("Name");

                    b.HasKey("TechnologyGuid");

                    b.ToTable("Technologies");
                });

            modelBuilder.Entity("Library.Database.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("NormalizedUserName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Library.Database.Models.Visitor.Visitor", b =>
                {
                    b.Property<Guid>("VisitorGuid")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("HostAdress");

                    b.Property<string>("PageUrl");

                    b.Property<string>("UserAgent");

                    b.Property<DateTimeOffset>("VisitTime");

                    b.HasKey("VisitorGuid");

                    b.ToTable("Visitors");
                });

            modelBuilder.Entity("Library.Database.Models.Book", b =>
                {
                    b.HasOne("Library.Database.Models.Author", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorGuid1");

                    b.HasOne("Library.Database.Models.Publishing", "Publishing")
                        .WithMany()
                        .HasForeignKey("PublishingGuid")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Library.Database.Models.Technology", "Technology")
                        .WithMany()
                        .HasForeignKey("TechnologyGuid")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Library.Database.Models.RSS.RssItem", b =>
                {
                    b.HasOne("Library.Database.Models.RSS.RssSource", "RssSource")
                        .WithMany()
                        .HasForeignKey("RssSourceGuid")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
