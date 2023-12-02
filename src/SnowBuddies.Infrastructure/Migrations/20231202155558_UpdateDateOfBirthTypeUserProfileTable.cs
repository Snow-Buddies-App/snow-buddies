﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnowBuddies.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDateOfBirthTypeUserProfileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "DateOfBirth",
                table: "UserProfiles",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "UserProfiles",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
