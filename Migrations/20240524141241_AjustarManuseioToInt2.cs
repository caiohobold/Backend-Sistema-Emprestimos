﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmprestimosAPI.Migrations
{
    /// <inheritdoc />
    public partial class AjustarManuseioToInt2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"ALTER TABLE ""Equipamentos"" 
                  ALTER COLUMN carga_equipamento 
                  TYPE integer 
                  USING 
                  CASE 
                    WHEN carga_equipamento = 'Até 80kg' THEN 1 
                    WHEN carga_equipamento = 'Até 120kg' THEN 2 
                    WHEN carga_equipamento = 'Até ' THEN 3 
                    ELSE NULL 
                  END;"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "carga_equipamento",
                table: "Equipamentos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldMaxLength: 20);
        }
    }
}
