using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace App.Migrations
{
    public partial class InititalCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "banks",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    account_number = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_banks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "levels",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    required_exp = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_levels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vouchers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<string>(type: "character varying(48)", nullable: false),
                    updated_at = table.Column<string>(type: "character varying(48)", nullable: false),
                    is_used = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vouchers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "text", nullable: false),
                    encrypted_password = table.Column<string>(type: "text", nullable: false),
                    display_name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    balance = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    exp = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    levelId = table.Column<long>(type: "bigint", nullable: false),
                    login_type = table.Column<string>(type: "text", nullable: false, defaultValue: "Standard"),
                    user_role = table.Column<string>(type: "text", nullable: false, defaultValue: "Customer")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_levels_levelId",
                        column: x => x.levelId,
                        principalTable: "levels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bank_topup_request",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<string>(type: "character varying(48)", nullable: false),
                    updated_at = table.Column<string>(type: "character varying(48)", nullable: false),
                    expired_date = table.Column<string>(type: "character varying(48)", nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "Pending"),
                    from_user_id = table.Column<long>(type: "bigint", nullable: false),
                    bank_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bank_topup_request", x => x.id);
                    table.ForeignKey(
                        name: "FK_bank_topup_request_banks_bank_id",
                        column: x => x.bank_id,
                        principalTable: "banks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bank_topup_request_users_from_user_id",
                        column: x => x.from_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transaction_histories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<string>(type: "character varying(48)", nullable: false),
                    updated_at = table.Column<string>(type: "character varying(48)", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    from_user_id = table.Column<long>(type: "bigint", nullable: false),
                    to_user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_histories", x => x.id);
                    table.ForeignKey(
                        name: "FK_transaction_histories_users_from_user_id",
                        column: x => x.from_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transaction_histories_users_to_user_id",
                        column: x => x.to_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "topup_histories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<string>(type: "character varying(48)", nullable: false),
                    updated_at = table.Column<string>(type: "character varying(48)", nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false),
                    method = table.Column<string>(type: "text", nullable: false),
                    from_user_id = table.Column<long>(type: "bigint", nullable: false),
                    bank_request_id = table.Column<long>(type: "bigint", nullable: true),
                    voucher_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topup_histories", x => x.id);
                    table.ForeignKey(
                        name: "FK_topup_histories_bank_topup_request_bank_request_id",
                        column: x => x.bank_request_id,
                        principalTable: "bank_topup_request",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_topup_histories_users_from_user_id",
                        column: x => x.from_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_topup_histories_vouchers_voucher_id",
                        column: x => x.voucher_id,
                        principalTable: "vouchers",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                table: "banks",
                columns: new[] { "id", "account_number", "name" },
                values: new object[] { 1L, 999999L, "TESTBANK" });

            migrationBuilder.InsertData(
                table: "levels",
                columns: new[] { "id", "name" },
                values: new object[] { 1L, "Bronze" });

            migrationBuilder.InsertData(
                table: "levels",
                columns: new[] { "id", "name", "required_exp" },
                values: new object[,]
                {
                    { 2L, "Silver", 100L },
                    { 3L, "Gold", 200L },
                    { 4L, "Platinum", 300L },
                    { 5L, "Diamond", 400L },
                    { 6L, "Crazy Rich", 500L }
                });

            migrationBuilder.InsertData(
                table: "vouchers",
                columns: new[] { "id", "amount", "code", "created_at", "updated_at" },
                values: new object[] { 1L, 50000L, "ZZZZZZ", "2022-04-12 22:56:23.045609", "2022-04-12 22:56:23.045609" });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "display_name", "email", "encrypted_password", "levelId", "user_role", "username" },
                values: new object[] { 1L, "Admin", "admin@cakrawala.id", "$2a$11$uauuW3lKAbyZ1T4pL7qLMeEurg5dAy0pE7HIEcp6eNxT4jrP6GjLy", 1L, "Admin", "cakrawalaid" });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "display_name", "email", "encrypted_password", "levelId", "username" },
                values: new object[] { 2L, "tes1", "tes1@cakrawala.id", "$2a$11$ir4erFZDv.M5lT8jzdtTy.Jf4rt1BGkoh.vUc2y3gMeKIRy.yVnsm", 1L, "tes1" });

            migrationBuilder.InsertData(
                table: "bank_topup_request",
                columns: new[] { "id", "amount", "bank_id", "created_at", "expired_date", "from_user_id", "status", "updated_at" },
                values: new object[] { 1L, 50000, 1L, "2022-04-12 22:56:23.04553", "2022-04-13 22:56:23.04553", 1L, "Success", "2022-04-12 22:56:23.04553" });

            migrationBuilder.InsertData(
                table: "topup_histories",
                columns: new[] { "id", "amount", "bank_request_id", "created_at", "from_user_id", "method", "updated_at", "voucher_id" },
                values: new object[] { 1L, 5000, null, "2022-04-12 22:56:23.045401", 1L, "Voucher", "2022-04-12 22:56:23.045402", 1L });

            migrationBuilder.InsertData(
                table: "transaction_histories",
                columns: new[] { "id", "amount", "created_at", "from_user_id", "status", "to_user_id", "updated_at" },
                values: new object[,]
                {
                    { 1L, 5000L, "2022-04-12 22:56:23.046833", 1L, "Success", 2L, "2022-04-12 22:56:23.046833" },
                    { 2L, 5000L, "2022-04-12 22:56:23.046833", 2L, "Success", 1L, "2022-04-12 22:56:23.046833" }
                });

            migrationBuilder.InsertData(
                table: "topup_histories",
                columns: new[] { "id", "amount", "bank_request_id", "created_at", "from_user_id", "method", "updated_at", "voucher_id" },
                values: new object[] { 2L, 50000, 1L, "2022-04-12 22:56:23.045402", 1L, "Bank", "2022-04-12 22:56:23.045403", null });

            migrationBuilder.CreateIndex(
                name: "IX_bank_topup_request_bank_id",
                table: "bank_topup_request",
                column: "bank_id");

            migrationBuilder.CreateIndex(
                name: "IX_bank_topup_request_from_user_id",
                table: "bank_topup_request",
                column: "from_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_bank_topup_request_id",
                table: "bank_topup_request",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_levels_id",
                table: "levels",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_topup_histories_bank_request_id",
                table: "topup_histories",
                column: "bank_request_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_topup_histories_from_user_id",
                table: "topup_histories",
                column: "from_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_topup_histories_id",
                table: "topup_histories",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_topup_histories_voucher_id",
                table: "topup_histories",
                column: "voucher_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transaction_histories_from_user_id",
                table: "transaction_histories",
                column: "from_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_histories_id",
                table: "transaction_histories",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transaction_histories_to_user_id",
                table: "transaction_histories",
                column: "to_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_id",
                table: "users",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_levelId",
                table: "users",
                column: "levelId");

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vouchers_code",
                table: "vouchers",
                column: "code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "topup_histories");

            migrationBuilder.DropTable(
                name: "transaction_histories");

            migrationBuilder.DropTable(
                name: "bank_topup_request");

            migrationBuilder.DropTable(
                name: "vouchers");

            migrationBuilder.DropTable(
                name: "banks");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "levels");
        }
    }
}
