using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplicationApi.Migrations
{
    /// <inheritdoc />
    public partial class SeededDefaultUserandRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "afc22088-a6f4-40a4-8250-d933d35adce0", null, "User", "USER" },
                    { "f0482d99-62b3-44f3-86f9-52d079dc1085", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "076fbb6c-257f-45a3-8351-9a2c84cd54fe", 0, "9d558ce8-0f58-4622-bcbb-baf8b8ab6e46", "admin@bookstoore.com", false, "System", "Admin", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEPpYI5Qy1O9JL2i50ooBLa9u0lEkqDAuw1ttWxXD+GmEDK+eDy+F1STLtiGtSmXtKw==", null, false, "4a8592ec-4d6b-4f5e-a9fd-a8590da09c62", false, "admin@bookstoore.com" },
                    { "cdc3ca1a-e07c-4ed7-87c5-f6e229a5174f", 0, "3fcd4c8c-ed0f-42ed-a539-41cfd81193f3", "user@bookstoore.com", false, "System", "User", false, null, "USER@BOOKSTORE.COM", "USER@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEJcTiUFb0Bzl2ygk2ci8z8BaSrEeZ62/9xPKZqKkEhAtOgEKynhbaaOqnG/nYnuEQg==", null, false, "b0cee40a-4fad-48e8-a871-3de15afcb558", false, "user@bookstoore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "f0482d99-62b3-44f3-86f9-52d079dc1085", "076fbb6c-257f-45a3-8351-9a2c84cd54fe" },
                    { "afc22088-a6f4-40a4-8250-d933d35adce0", "cdc3ca1a-e07c-4ed7-87c5-f6e229a5174f" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f0482d99-62b3-44f3-86f9-52d079dc1085", "076fbb6c-257f-45a3-8351-9a2c84cd54fe" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "afc22088-a6f4-40a4-8250-d933d35adce0", "cdc3ca1a-e07c-4ed7-87c5-f6e229a5174f" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "afc22088-a6f4-40a4-8250-d933d35adce0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f0482d99-62b3-44f3-86f9-52d079dc1085");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "076fbb6c-257f-45a3-8351-9a2c84cd54fe");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cdc3ca1a-e07c-4ed7-87c5-f6e229a5174f");
        }
    }
}
