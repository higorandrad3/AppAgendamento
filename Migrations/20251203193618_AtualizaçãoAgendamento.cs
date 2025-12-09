using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgendamentoApp.Migrations
{
    /// <inheritdoc />
    public partial class AtualizaçãoAgendamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RepresentanteMunicipio",
                table: "Agendamentos",
                newName: "NomeRepresentanteMunicipio");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Municipios",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NomeRepresentanteMunicipio",
                table: "Agendamentos",
                newName: "RepresentanteMunicipio");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Municipios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
