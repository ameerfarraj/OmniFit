using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmniFit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTraineeProfileOnboarding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAgeVerified",
                table: "TraineeProfiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PreviousExperience",
                table: "TraineeProfiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrainingGoals",
                table: "TraineeProfiles",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAgeVerified",
                table: "TraineeProfiles");

            migrationBuilder.DropColumn(
                name: "PreviousExperience",
                table: "TraineeProfiles");

            migrationBuilder.DropColumn(
                name: "TrainingGoals",
                table: "TraineeProfiles");
        }
    }
}
