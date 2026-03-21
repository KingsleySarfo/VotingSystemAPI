using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingSystemAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCandidateImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Candidates");
        }
    }
}
