using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CitiesManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("08a84aab-9084-4179-9e9d-9d43de5546fc"), "Kyiv" },
                    { new Guid("0edbd081-15ef-4679-a5f4-5d000aaf2b4e"), "Prague" },
                    { new Guid("8467e78c-bf7d-443b-9e7d-e07848e6a6a4"), "Amsterdam" },
                    { new Guid("8f8c9266-193d-4a63-ac50-1db0954c9c5e"), "Chicago" },
                    { new Guid("b9bb7cd6-7969-4fc0-8dae-b7ef7c9c6580"), "Berlin" },
                    { new Guid("bcee3462-1512-4fc3-98cf-dbfbccf1e919"), "London" },
                    { new Guid("cdea0ec1-4f3a-4c0a-8804-a9acc278c78b"), "Atlanta" },
                    { new Guid("d9ee6f5f-5dbe-4356-8c78-af1532ac8fbc"), "Bangkok" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
