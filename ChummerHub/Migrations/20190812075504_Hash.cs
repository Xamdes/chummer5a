using Microsoft.EntityFrameworkCore.Migrations;

namespace ChummerHub.Migrations
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Hash'
    public partial class Hash : Migration
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'Hash'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Hash.Up(MigrationBuilder)'
        protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AddColumn<string>(
                name: "Hash",
                table: "SINners",
                nullable: true);

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'Hash.Down(MigrationBuilder)'
        protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropColumn(
                name: "Hash",
                table: "SINners");
    }
}
