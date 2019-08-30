using Microsoft.EntityFrameworkCore.Migrations;

namespace ChummerHub.Migrations
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'favGroupsIndex'
    public partial class favGroupsIndex : Migration
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'favGroupsIndex'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'favGroupsIndex.Up(MigrationBuilder)'
        protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserFavoriteGroup_FavoriteGuid",
                table: "ApplicationUserFavoriteGroup",
                column: "FavoriteGuid");

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'favGroupsIndex.Down(MigrationBuilder)'
        protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropIndex(
                name: "IX_ApplicationUserFavoriteGroup_FavoriteGuid",
                table: "ApplicationUserFavoriteGroup");
    }
}
