namespace WebApi.Db.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coffees",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UUID", nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    price = table.Column<decimal>(type: "MONEY", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coffees", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UUID", nullable: false),
                    login = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    password = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    balance = table.Column<decimal>(type: "MONEY", nullable: false),
                    refreshtoken = table.Column<string>(name: "refresh_token", type: "TEXT", nullable: true),
                    refreshtokenexpirytime = table.Column<DateTime>(name: "refresh_token_expiry_time", type: "DATE", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Statistics",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UUID", nullable: false),
                    FKStatisticsCoffees = table.Column<Guid>(name: "FK_Statistics_Coffees", type: "UUID", nullable: false),
                    total = table.Column<decimal>(type: "MONEY", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistics", x => x.id);
                    table.ForeignKey(
                        name: "FK_Statistics_Coffees",
                        column: x => x.FKStatisticsCoffees,
                        principalTable: "Coffees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UUID", nullable: false),
                    FKOrdersCoffees = table.Column<Guid>(name: "FK_Orders_Coffees", type: "UUID", nullable: false),
                    FKOrdersUsers = table.Column<Guid>(name: "FK_Orders_Users", type: "UUID", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.id);
                    table.ForeignKey(
                        name: "FK_Orders_Coffees",
                        column: x => x.FKOrdersCoffees,
                        principalTable: "Coffees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Users",
                        column: x => x.FKOrdersUsers,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_FK_Orders_Coffees",
                table: "Orders",
                column: "FK_Orders_Coffees");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_FK_Orders_Users",
                table: "Orders",
                column: "FK_Orders_Users");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_FK_Statistics_Coffees",
                table: "Statistics",
                column: "FK_Statistics_Coffees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Statistics");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Coffees");
        }
    }
}
