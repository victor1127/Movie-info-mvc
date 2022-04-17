using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MovieProDemo.Data.Migrations
{
    public partial class CreatingCollectionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Collections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TmDbMovieId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    TagLine = table.Column<string>(type: "text", nullable: true),
                    Overview = table.Column<string>(type: "text", nullable: true),
                    RunTime = table.Column<int>(type: "integer", nullable: false),
                    VoteAverage = table.Column<float>(type: "real", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    TrailerUrl = table.Column<string>(type: "text", nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    PosterImage = table.Column<byte[]>(type: "bytea", nullable: true),
                    BackDropImage = table.Column<byte[]>(type: "bytea", nullable: true),
                    PostImageType = table.Column<string>(type: "text", nullable: true),
                    BackDropImageType = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MovieCast",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MovieId = table.Column<int>(type: "integer", nullable: false),
                    CastId = table.Column<int>(type: "integer", nullable: false),
                    Department = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Character = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieCast", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieCast_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieCollection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MovieId = table.Column<int>(type: "integer", nullable: false),
                    CollectionId = table.Column<int>(type: "integer", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieCollection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieCollection_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieCollection_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieCrew",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MovieId = table.Column<int>(type: "integer", nullable: false),
                    CrewId = table.Column<int>(type: "integer", nullable: false),
                    Department = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Job = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieCrew", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieCrew_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieCast_MovieId",
                table: "MovieCast",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieCollection_CollectionId",
                table: "MovieCollection",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieCollection_MovieId",
                table: "MovieCollection",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieCrew_MovieId",
                table: "MovieCrew",
                column: "MovieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieCast");

            migrationBuilder.DropTable(
                name: "MovieCollection");

            migrationBuilder.DropTable(
                name: "MovieCrew");

            migrationBuilder.DropTable(
                name: "Collections");

            migrationBuilder.DropTable(
                name: "Movie");
        }
    }
}
