using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class mymigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChampionsSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Bio = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Class = table.Column<int>(type: "INTEGER", nullable: false),
                    Icon = table.Column<string>(type: "TEXT", nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChampionsSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RunePagesSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunePagesSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RunesSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Family = table.Column<int>(type: "INTEGER", nullable: false),
                    Icon = table.Column<string>(type: "TEXT", nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunesSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CaracteristicSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    key = table.Column<string>(type: "TEXT", nullable: false),
                    valeur = table.Column<int>(type: "INTEGER", nullable: false),
                    ChampionForeignKey = table.Column<int>(type: "INTEGER", nullable: false),
                    championId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaracteristicSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaracteristicSet_ChampionsSet_championId",
                        column: x => x.championId,
                        principalTable: "ChampionsSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkillSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    ChampionForeignKey = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillSet_ChampionsSet_ChampionForeignKey",
                        column: x => x.ChampionForeignKey,
                        principalTable: "ChampionsSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkinsSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Icon = table.Column<string>(type: "TEXT", nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<float>(type: "REAL", nullable: false),
                    ChampionForeignKey = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkinsSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkinsSet_ChampionsSet_ChampionForeignKey",
                        column: x => x.ChampionForeignKey,
                        principalTable: "ChampionsSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChampionDBRunePagesDb",
                columns: table => new
                {
                    RunePagesId = table.Column<int>(type: "INTEGER", nullable: false),
                    championsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChampionDBRunePagesDb", x => new { x.RunePagesId, x.championsId });
                    table.ForeignKey(
                        name: "FK_ChampionDBRunePagesDb_ChampionsSet_championsId",
                        column: x => x.championsId,
                        principalTable: "ChampionsSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChampionDBRunePagesDb_RunePagesSet_RunePagesId",
                        column: x => x.RunePagesId,
                        principalTable: "RunePagesSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryDicSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    category = table.Column<int>(type: "INTEGER", nullable: false),
                    runesPagesForeignKey = table.Column<int>(type: "INTEGER", nullable: false),
                    runeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryDicSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryDicSet_RunePagesSet_runesPagesForeignKey",
                        column: x => x.runesPagesForeignKey,
                        principalTable: "RunePagesSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryDicSet_RunesSet_runeId",
                        column: x => x.runeId,
                        principalTable: "RunesSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaracteristicSet_championId",
                table: "CaracteristicSet",
                column: "championId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryDicSet_category",
                table: "CategoryDicSet",
                column: "category",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryDicSet_runeId",
                table: "CategoryDicSet",
                column: "runeId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryDicSet_runesPagesForeignKey",
                table: "CategoryDicSet",
                column: "runesPagesForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_ChampionDBRunePagesDb_championsId",
                table: "ChampionDBRunePagesDb",
                column: "championsId");

            migrationBuilder.CreateIndex(
                name: "IX_ChampionsSet_Name",
                table: "ChampionsSet",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SkillSet_ChampionForeignKey",
                table: "SkillSet",
                column: "ChampionForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_SkinsSet_ChampionForeignKey",
                table: "SkinsSet",
                column: "ChampionForeignKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaracteristicSet");

            migrationBuilder.DropTable(
                name: "CategoryDicSet");

            migrationBuilder.DropTable(
                name: "ChampionDBRunePagesDb");

            migrationBuilder.DropTable(
                name: "SkillSet");

            migrationBuilder.DropTable(
                name: "SkinsSet");

            migrationBuilder.DropTable(
                name: "RunesSet");

            migrationBuilder.DropTable(
                name: "RunePagesSet");

            migrationBuilder.DropTable(
                name: "ChampionsSet");
        }
    }
}
