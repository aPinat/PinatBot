using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PinatBot.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "general_logging_configs",
                columns: table => new
                {
                    guild_id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    enabled = table.Column<bool>(type: "boolean", nullable: false),
                    channel_id = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_general_logging_configs", x => x.guild_id);
                });

            migrationBuilder.CreateTable(
                name: "member_join_role_configs",
                columns: table => new
                {
                    guild_id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    enabled = table.Column<bool>(type: "boolean", nullable: false),
                    role_id = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_member_join_role_configs", x => x.guild_id);
                });

            migrationBuilder.CreateTable(
                name: "voice_state_logging_configs",
                columns: table => new
                {
                    guild_id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    enabled = table.Column<bool>(type: "boolean", nullable: false),
                    channel_id = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_voice_state_logging_configs", x => x.guild_id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_general_logging_configs_guild_id",
                table: "general_logging_configs",
                column: "guild_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_member_join_role_configs_guild_id",
                table: "member_join_role_configs",
                column: "guild_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_voice_state_logging_configs_guild_id",
                table: "voice_state_logging_configs",
                column: "guild_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "general_logging_configs");

            migrationBuilder.DropTable(
                name: "member_join_role_configs");

            migrationBuilder.DropTable(
                name: "voice_state_logging_configs");
        }
    }
}
