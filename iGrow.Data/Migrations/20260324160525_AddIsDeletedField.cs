using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iGrow.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Tasks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Habits",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Habits");
        }
    }
}
