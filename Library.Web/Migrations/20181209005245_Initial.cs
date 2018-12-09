using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Web.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorGuid = table.Column<Guid>(nullable: false),
                    Surname = table.Column<string>(maxLength: 100, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Patronymic = table.Column<string>(maxLength: 100, nullable: true),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.AuthorGuid);
                });

            migrationBuilder.CreateTable(
                name: "Publishings",
                columns: table => new
                {
                    PublishingGuid = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Postcode = table.Column<long>(nullable: false),
                    Street = table.Column<string>(nullable: true),
                    House = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publishings", x => x.PublishingGuid);
                });

            migrationBuilder.CreateTable(
                name: "RssSources",
                columns: table => new
                {
                    RssSourceGuid = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Uri = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RssSources", x => x.RssSourceGuid);
                });

            migrationBuilder.CreateTable(
                name: "Technologies",
                columns: table => new
                {
                    TechnologyGuid = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Technologies", x => x.TechnologyGuid);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Visitors",
                columns: table => new
                {
                    VisitorGuid = table.Column<Guid>(nullable: false),
                    HostAdress = table.Column<string>(nullable: true),
                    UserAgent = table.Column<string>(nullable: true),
                    PageUrl = table.Column<string>(nullable: true),
                    VisitTime = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitors", x => x.VisitorGuid);
                });

            migrationBuilder.CreateTable(
                name: "RssItems",
                columns: table => new
                {
                    RssItemGuid = table.Column<Guid>(nullable: false),
                    RssSourceGuid = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    PubDate = table.Column<DateTimeOffset>(nullable: false),
                    Enclosure = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RssItems", x => x.RssItemGuid);
                    table.ForeignKey(
                        name: "FK_RssItems_RssSources_RssSourceGuid",
                        column: x => x.RssSourceGuid,
                        principalTable: "RssSources",
                        principalColumn: "RssSourceGuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookGuid = table.Column<Guid>(nullable: false),
                    AuthorGuid = table.Column<List<Guid>>(nullable: true),
                    PublishingGuid = table.Column<Guid>(nullable: false),
                    TechnologyGuid = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Annotation = table.Column<string>(nullable: true),
                    Cover = table.Column<string>(nullable: true),
                    NumberOfPages = table.Column<int>(nullable: false),
                    Format = table.Column<string>(nullable: true),
                    Cost = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    BuyUri = table.Column<string>(nullable: true),
                    ImageUri = table.Column<string>(nullable: true),
                    CreationDateTimeOffset = table.Column<DateTimeOffset>(nullable: false),
                    AuthorGuid1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookGuid);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorGuid1",
                        column: x => x.AuthorGuid1,
                        principalTable: "Authors",
                        principalColumn: "AuthorGuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Books_Publishings_PublishingGuid",
                        column: x => x.PublishingGuid,
                        principalTable: "Publishings",
                        principalColumn: "PublishingGuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_Technologies_TechnologyGuid",
                        column: x => x.TechnologyGuid,
                        principalTable: "Technologies",
                        principalColumn: "TechnologyGuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorGuid1",
                table: "Books",
                column: "AuthorGuid1");

            migrationBuilder.CreateIndex(
                name: "IX_Books_PublishingGuid",
                table: "Books",
                column: "PublishingGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Books_TechnologyGuid",
                table: "Books",
                column: "TechnologyGuid");

            migrationBuilder.CreateIndex(
                name: "IX_RssItems_RssSourceGuid",
                table: "RssItems",
                column: "RssSourceGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "RssItems");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Visitors");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Publishings");

            migrationBuilder.DropTable(
                name: "Technologies");

            migrationBuilder.DropTable(
                name: "RssSources");
        }
    }
}
