using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgendamentoApp.Migrations
{
    /// <inheritdoc />
    public partial class AtualizaAgendamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendamentos_Municipios_MunicipioId",
                table: "Agendamentos");

            migrationBuilder.AlterColumn<int>(
                name: "MunicipioId",
                table: "Agendamentos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Municipio",
                table: "Agendamentos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamentos_Municipios_MunicipioId",
                table: "Agendamentos",
                column: "MunicipioId",
                principalTable: "Municipios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendamentos_Municipios_MunicipioId",
                table: "Agendamentos");

            migrationBuilder.DropColumn(
                name: "Municipio",
                table: "Agendamentos");

            migrationBuilder.AlterColumn<int>(
                name: "MunicipioId",
                table: "Agendamentos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamentos_Municipios_MunicipioId",
                table: "Agendamentos",
                column: "MunicipioId",
                principalTable: "Municipios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
