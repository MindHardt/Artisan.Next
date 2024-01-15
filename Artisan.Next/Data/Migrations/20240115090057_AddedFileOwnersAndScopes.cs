using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artisan.Next.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedFileOwnersAndScopes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateCreated",
                table: "Files",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateUpdated",
                table: "Files",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Files",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Scope",
                table: "Files",
                type: "character varying(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Files",
                keyColumn: "UniqueName",
                keyValue: "DefaultAvatar.jpg",
                columns: new[] { "OwnerId", "Scope" },
                values: new object[] { null, "Avatar" });

            migrationBuilder.CreateIndex(
                name: "IX_Files_OwnerId_Scope",
                table: "Files",
                columns: new[] { "OwnerId", "Scope" });

            migrationBuilder.AddForeignKey(
                name: "FK_Files_AspNetUsers_OwnerId",
                table: "Files",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_AspNetUsers_OwnerId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_OwnerId_Scope",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Scope",
                table: "Files");
        }
    }
}
