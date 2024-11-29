using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addCity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "ToDoItems",
                type: "datetimeoffset",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2024, 11, 29, 4, 32, 59, 447, DateTimeKind.Unspecified).AddTicks(3030), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2024, 9, 23, 10, 28, 25, 719, DateTimeKind.Unspecified).AddTicks(7530), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Persons",
                type: "datetimeoffset",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2024, 11, 29, 4, 32, 59, 447, DateTimeKind.Unspecified).AddTicks(890), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2024, 9, 23, 10, 28, 25, 719, DateTimeKind.Unspecified).AddTicks(5520), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "ToDoItems",
                type: "datetimeoffset",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2024, 9, 23, 10, 28, 25, 719, DateTimeKind.Unspecified).AddTicks(7530), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2024, 11, 29, 4, 32, 59, 447, DateTimeKind.Unspecified).AddTicks(3030), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "Persons",
                type: "datetimeoffset",
                nullable: true,
                defaultValue: new DateTimeOffset(new DateTime(2024, 9, 23, 10, 28, 25, 719, DateTimeKind.Unspecified).AddTicks(5520), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldDefaultValue: new DateTimeOffset(new DateTime(2024, 11, 29, 4, 32, 59, 447, DateTimeKind.Unspecified).AddTicks(890), new TimeSpan(0, 7, 0, 0, 0)));
        }
    }
}
