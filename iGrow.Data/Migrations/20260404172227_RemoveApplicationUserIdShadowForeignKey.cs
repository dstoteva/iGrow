using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iGrow.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveApplicationUserIdShadowForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Habits_AspNetUsers_ApplicationUserId",
                table: "Habits");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_ApplicationUserId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_ApplicationUserId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Habits_ApplicationUserId",
                table: "Habits");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Habits");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "Tasks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "Habits",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ApplicationUserId",
                table: "Tasks",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Habits_ApplicationUserId",
                table: "Habits",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Habits_AspNetUsers_ApplicationUserId",
                table: "Habits",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_ApplicationUserId",
                table: "Tasks",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
