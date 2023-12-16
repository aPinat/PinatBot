using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PinatBot.Data.Migrations
{
    /// <inheritdoc />
    public partial class AutoVoiceChannels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "auto_voice_channels",
                columns: table => new
                {
                    channel_id = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_auto_voice_channels", x => x.channel_id);
                });

            migrationBuilder.CreateTable(
                name: "new_session_channels",
                columns: table => new
                {
                    channel_id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    child_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_new_session_channels", x => x.channel_id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_auto_voice_channels_channel_id",
                table: "auto_voice_channels",
                column: "channel_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_new_session_channels_channel_id",
                table: "new_session_channels",
                column: "channel_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "auto_voice_channels");

            migrationBuilder.DropTable(
                name: "new_session_channels");
        }
    }
}
