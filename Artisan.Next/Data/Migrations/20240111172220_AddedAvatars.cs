using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artisan.Next.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedAvatars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Files",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_UniqueName",
                table: "Files");

            migrationBuilder.DeleteData(
                table: "Files",
                keyColumn: "Id",
                keyColumnType: "uuid",
                keyValue: new Guid("854fd069-07a5-48d6-93f8-5a3224c05634"));

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Files");

            migrationBuilder.AddColumn<string>(
                name: "AvatarName",
                table: "AspNetUsers",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "DefaultAvatar.jpg");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Files",
                table: "Files",
                column: "UniqueName");

            migrationBuilder.InsertData(
                table: "Files",
                columns: new[] { "UniqueName", "MimeType", "OriginalName" },
                values: new object[] { "DefaultAvatar.jpg", "image/jpeg", "DefaultAvatar.jpg" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AvatarName",
                table: "AspNetUsers",
                column: "AvatarName");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Files_AvatarName",
                table: "AspNetUsers",
                column: "AvatarName",
                principalTable: "Files",
                principalColumn: "UniqueName",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Files_AvatarName",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Files",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AvatarName",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "Files",
                keyColumn: "UniqueName",
                keyValue: "DefaultAvatar.jpg");

            migrationBuilder.DropColumn(
                name: "AvatarName",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Files",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Files",
                table: "Files",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Files",
                columns: new[] { "Id", "MimeType", "OriginalName", "UniqueName" },
                values: new object[] { new Guid("854fd069-07a5-48d6-93f8-5a3224c05634"), "image/jpeg", "DefaultAvatar.jpg", "DefaultAvatar.jpg" });

            migrationBuilder.CreateIndex(
                name: "IX_Files_UniqueName",
                table: "Files",
                column: "UniqueName",
                unique: true);
        }
    }
}
