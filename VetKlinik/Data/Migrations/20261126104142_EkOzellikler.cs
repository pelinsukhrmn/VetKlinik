using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetKlinik.Data.Migrations
{
    /// <inheritdoc />
    public partial class EkOzellikler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AsiSablonuId",
                table: "AsiKayitlari",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AsiSablonlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AsiAdi = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Tur = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    TekrarAraligiGun = table.Column<int>(type: "INTEGER", nullable: true),
                    Aciklama = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    AktifMi = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsiSablonlari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KiloOlcumleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EvcilHayvanId = table.Column<int>(type: "INTEGER", nullable: false),
                    OlcumTarihi = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Kilo = table.Column<double>(type: "REAL", nullable: false),
                    RandevuId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KiloOlcumleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KiloOlcumleri_EvcilHayvanlar_EvcilHayvanId",
                        column: x => x.EvcilHayvanId,
                        principalTable: "EvcilHayvanlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KiloOlcumleri_Randevular_RandevuId",
                        column: x => x.RandevuId,
                        principalTable: "Randevular",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "RandevuDurumGecmisleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RandevuId = table.Column<int>(type: "INTEGER", nullable: false),
                    EskiDurum = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    YeniDurum = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    DegistirenKullaniciId = table.Column<string>(type: "TEXT", nullable: false),
                    Tarih = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Aciklama = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RandevuDurumGecmisleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RandevuDurumGecmisleri_AspNetUsers_DegistirenKullaniciId",
                        column: x => x.DegistirenKullaniciId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RandevuDurumGecmisleri_Randevular_RandevuId",
                        column: x => x.RandevuId,
                        principalTable: "Randevular",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VeterinerIzinleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VeterinerId = table.Column<int>(type: "INTEGER", nullable: false),
                    BaslangicTarihi = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    BitisTarihi = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Aciklama = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VeterinerIzinleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VeterinerIzinleri_Veterinerler_VeterinerId",
                        column: x => x.VeterinerId,
                        principalTable: "Veterinerler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AsiKayitlari_AsiSablonuId",
                table: "AsiKayitlari",
                column: "AsiSablonuId");

            migrationBuilder.CreateIndex(
                name: "IX_AsiSablonlari_Tur_AsiAdi",
                table: "AsiSablonlari",
                columns: new[] { "Tur", "AsiAdi" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KiloOlcumleri_EvcilHayvanId",
                table: "KiloOlcumleri",
                column: "EvcilHayvanId");

            migrationBuilder.CreateIndex(
                name: "IX_KiloOlcumleri_RandevuId",
                table: "KiloOlcumleri",
                column: "RandevuId");

            migrationBuilder.CreateIndex(
                name: "IX_RandevuDurumGecmisleri_DegistirenKullaniciId",
                table: "RandevuDurumGecmisleri",
                column: "DegistirenKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_RandevuDurumGecmisleri_RandevuId",
                table: "RandevuDurumGecmisleri",
                column: "RandevuId");

            migrationBuilder.CreateIndex(
                name: "IX_VeterinerIzinleri_VeterinerId",
                table: "VeterinerIzinleri",
                column: "VeterinerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AsiKayitlari_AsiSablonlari_AsiSablonuId",
                table: "AsiKayitlari",
                column: "AsiSablonuId",
                principalTable: "AsiSablonlari",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AsiKayitlari_AsiSablonlari_AsiSablonuId",
                table: "AsiKayitlari");

            migrationBuilder.DropTable(
                name: "AsiSablonlari");

            migrationBuilder.DropTable(
                name: "KiloOlcumleri");

            migrationBuilder.DropTable(
                name: "RandevuDurumGecmisleri");

            migrationBuilder.DropTable(
                name: "VeterinerIzinleri");

            migrationBuilder.DropIndex(
                name: "IX_AsiKayitlari_AsiSablonuId",
                table: "AsiKayitlari");

            migrationBuilder.DropColumn(
                name: "AsiSablonuId",
                table: "AsiKayitlari");
        }
    }
}
