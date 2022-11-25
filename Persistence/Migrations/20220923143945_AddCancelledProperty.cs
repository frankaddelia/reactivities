﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
  public partial class AddCancelledProperty : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<bool>(
          name: "IsCancelled",
          table: "Activities",
          type: "BOOLEAN",
          nullable: false,
          defaultValue: false);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "IsCancelled",
          table: "Activities");
    }
  }
}
