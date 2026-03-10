using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeThongTuyenDung.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationFormFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiaChi",
                table: "DonUngTuyens",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DuAn",
                table: "DonUngTuyens",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "DonUngTuyens",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoTen",
                table: "DonUngTuyens",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HocVan",
                table: "DonUngTuyens",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KinhNghiem",
                table: "DonUngTuyens",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KinhNghiemLamViec",
                table: "DonUngTuyens",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KyNang",
                table: "DonUngTuyens",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LuongMongMuon",
                table: "DonUngTuyens",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LyDoUngTuyen",
                table: "DonUngTuyens",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MucTieu",
                table: "DonUngTuyens",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoDienThoai",
                table: "DonUngTuyens",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianBatDau",
                table: "DonUngTuyens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThongTinBoSung",
                table: "DonUngTuyens",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TinTuyenDungs_DuongDan",
                table: "TinTuyenDungs",
                column: "DuongDan",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TinTuyenDungs_DuongDan",
                table: "TinTuyenDungs");

            migrationBuilder.DropColumn(
                name: "DiaChi",
                table: "DonUngTuyens");

            migrationBuilder.DropColumn(
                name: "DuAn",
                table: "DonUngTuyens");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "DonUngTuyens");

            migrationBuilder.DropColumn(
                name: "HoTen",
                table: "DonUngTuyens");

            migrationBuilder.DropColumn(
                name: "HocVan",
                table: "DonUngTuyens");

            migrationBuilder.DropColumn(
                name: "KinhNghiem",
                table: "DonUngTuyens");

            migrationBuilder.DropColumn(
                name: "KinhNghiemLamViec",
                table: "DonUngTuyens");

            migrationBuilder.DropColumn(
                name: "KyNang",
                table: "DonUngTuyens");

            migrationBuilder.DropColumn(
                name: "LuongMongMuon",
                table: "DonUngTuyens");

            migrationBuilder.DropColumn(
                name: "LyDoUngTuyen",
                table: "DonUngTuyens");

            migrationBuilder.DropColumn(
                name: "MucTieu",
                table: "DonUngTuyens");

            migrationBuilder.DropColumn(
                name: "SoDienThoai",
                table: "DonUngTuyens");

            migrationBuilder.DropColumn(
                name: "ThoiGianBatDau",
                table: "DonUngTuyens");

            migrationBuilder.DropColumn(
                name: "ThongTinBoSung",
                table: "DonUngTuyens");
        }
    }
}
