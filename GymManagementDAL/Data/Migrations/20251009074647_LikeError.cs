using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagementDAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class LikeError : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "GymUserValidEmailCheck1",
                table: "Trainers",
                sql: "(Email LIKE '_%@_%._%')");

            migrationBuilder.AddCheckConstraint(
                name: "GymUserValidPhoneCheck1",
                table: "Trainers",
                sql: "(Phone LIKE '01%' AND Phone NOT LIKE '%[^0-9]%')");

            migrationBuilder.AddCheckConstraint(
                name: "GymUserValidEmailCheck",
                table: "Members",
                sql: "(Email LIKE '_%@_%._%')");

            migrationBuilder.AddCheckConstraint(
                name: "GymUserValidPhoneCheck",
                table: "Members",
                sql: "(Phone LIKE '01%' AND Phone NOT LIKE '%[^0-9]%')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "GymUserValidEmailCheck1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "GymUserValidPhoneCheck1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "GymUserValidEmailCheck",
                table: "Members");

            migrationBuilder.DropCheckConstraint(
                name: "GymUserValidPhoneCheck",
                table: "Members");
        }
    }
}
