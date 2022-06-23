using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrgChartEngines.Migrations.Migrations_WeldContext
{
    public partial class Change_ColName_TblLine2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departament",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartamentName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departament", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Line",
                columns: table => new
                {
                    line_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    line_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Line", x => x.line_id);
                });

            migrationBuilder.CreateTable(
                name: "OrgChart_Positions",
                columns: table => new
                {
                    IdPosition = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IdRol = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IdUser = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IdDepartment = table.Column<int>(type: "int", nullable: true),
                    PositionLevel = table.Column<int>(type: "int", nullable: true),
                    SuperiorPosition = table.Column<int>(type: "int", nullable: true),
                    IdLine = table.Column<int>(type: "int", nullable: true),
                    IdLineSub = table.Column<int>(type: "int", nullable: true),
                    Assemblies = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgChart_Positions", x => x.IdPosition);
                });

            migrationBuilder.CreateTable(
                name: "RolesDepartments",
                columns: table => new
                {
                    IdRolDepartment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRol = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IdDepartment = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesDepartments", x => x.IdRolDepartment);
                    table.ForeignKey(
                        name: "FK_RolesDepartments_Departament",
                        column: x => x.IdDepartment,
                        principalTable: "Departament",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PositionUser",
                columns: table => new
                {
                    IdPositionUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPosition = table.Column<int>(type: "int", nullable: true),
                    IdUser = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Shift = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionUser", x => x.IdPositionUser);
                    table.ForeignKey(
                        name: "FK_PositionUser_OrgChart_Positions",
                        column: x => x.IdPosition,
                        principalTable: "OrgChart_Positions",
                        principalColumn: "IdPosition");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PositionUser_IdPosition",
                table: "PositionUser",
                column: "IdPosition");

            migrationBuilder.CreateIndex(
                name: "IX_RolesDepartments_IdDepartment",
                table: "RolesDepartments",
                column: "IdDepartment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Line");

            migrationBuilder.DropTable(
                name: "PositionUser");

            migrationBuilder.DropTable(
                name: "RolesDepartments");

            migrationBuilder.DropTable(
                name: "OrgChart_Positions");

            migrationBuilder.DropTable(
                name: "Departament");
        }
    }
}
