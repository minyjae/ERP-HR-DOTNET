using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLeavePolicy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "leave_policies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LeaveTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntitledDays = table.Column<int>(type: "integer", nullable: false),
                    MaxCarryOverDays = table.Column<int>(type: "integer", nullable: false),
                    MinServiceMonths = table.Column<int>(type: "integer", nullable: false),
                    AdvanceNoticeDays = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leave_policies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_leave_policies_leave_type",
                table: "leave_policies",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "ux_leave_policies_active",
                table: "leave_policies",
                column: "LeaveTypeId",
                unique: true,
                filter: "\"IsActive\" = true");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "leave_policies");
        }
    }
}
