using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Sync.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sync");

            migrationBuilder.CreateTable(
                name: "InboxMessages",
                schema: "sync",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OccuredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "json", nullable: false),
                    ProcessedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InboxMessagesConsumers",
                schema: "sync",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxMessagesConsumers", x => new { x.Id, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                schema: "sync",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OccuredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "json", nullable: false),
                    ProcessedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessagesConsumers",
                schema: "sync",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessagesConsumers", x => new { x.Id, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "ServiceAccounts",
                schema: "sync",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HubId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceAccountSyncStates",
                schema: "sync",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceAccountSyncStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceAccountSyncStates_ServiceAccounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "sync",
                        principalTable: "ServiceAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoogleServiceAccountSyncStates",
                schema: "sync",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HistoryId = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleServiceAccountSyncStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoogleServiceAccountSyncStates_ServiceAccountSyncStates_Id",
                        column: x => x.Id,
                        principalSchema: "sync",
                        principalTable: "ServiceAccountSyncStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAccounts_HubId",
                schema: "sync",
                table: "ServiceAccounts",
                column: "HubId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAccountSyncStates_AccountId",
                schema: "sync",
                table: "ServiceAccountSyncStates",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoogleServiceAccountSyncStates",
                schema: "sync");

            migrationBuilder.DropTable(
                name: "InboxMessages",
                schema: "sync");

            migrationBuilder.DropTable(
                name: "InboxMessagesConsumers",
                schema: "sync");

            migrationBuilder.DropTable(
                name: "OutboxMessages",
                schema: "sync");

            migrationBuilder.DropTable(
                name: "OutboxMessagesConsumers",
                schema: "sync");

            migrationBuilder.DropTable(
                name: "ServiceAccountSyncStates",
                schema: "sync");

            migrationBuilder.DropTable(
                name: "ServiceAccounts",
                schema: "sync");
        }
    }
}
