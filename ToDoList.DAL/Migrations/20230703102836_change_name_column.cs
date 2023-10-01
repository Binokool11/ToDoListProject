using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.DAL.Migrations
{
    /// <inheritdoc />
    public partial class change_name_column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Discription",
                table: "Tasks",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Tasks",
                newName: "Discription");
        }
    }
}
