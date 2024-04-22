﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MGOBankApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreatingDateTimeForCards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatingTime",
                table: "Cards",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatingTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatingTime",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CreatingTime",
                table: "AspNetUsers");
        }
    }
}
