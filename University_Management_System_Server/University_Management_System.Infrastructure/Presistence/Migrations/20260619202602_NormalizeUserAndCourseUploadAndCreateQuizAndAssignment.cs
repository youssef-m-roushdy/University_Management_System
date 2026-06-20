using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University_Management_System.Infrastructure.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeUserAndCourseUploadAndCreateQuizAndAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicSchedules_AspNetUsers_UploadedByUserId",
                table: "AcademicSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Departments_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Specializations_SpecializationId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseUploads_AspNetUsers_UploadedByUserId",
                table: "CourseUploads");

            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_AspNetUsers_UserId",
                table: "Registrations");

            migrationBuilder.DropForeignKey(
                name: "FK_SemesterGPAs_AspNetUsers_UserId",
                table: "SemesterGPAs");

            migrationBuilder.DropTable(
                name: "UserStudyYears");

            migrationBuilder.DropIndex(
                name: "IX_CourseUploads_UploadedByUserId",
                table: "CourseUploads");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AcademicCode",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SpecializationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "CourseUploads");

            migrationBuilder.DropColumn(
                name: "UploadedByUserId",
                table: "CourseUploads");

            migrationBuilder.DropColumn(
                name: "AcademicCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AllowedCredits",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SpecializationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TotalCredits",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TotalGPA",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "AcademicSchedules");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "SemesterGPAs",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_SemesterGPAs_UserId_SemesterId_StudyYearId",
                table: "SemesterGPAs",
                newName: "IX_SemesterGPAs_StudentId_SemesterId_StudyYearId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Registrations",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Registrations_UserId",
                table: "Registrations",
                newName: "IX_Registrations_StudentId");

            migrationBuilder.RenameColumn(
                name: "UploadedAt",
                table: "CourseUploads",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "DisplayName",
                table: "AspNetUsers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "UploadedByUserId",
                table: "AcademicSchedules",
                newName: "AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_AcademicSchedules_UploadedByUserId",
                table: "AcademicSchedules",
                newName: "IX_AcademicSchedules_AdminId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "StudyYears",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "StudyYears",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Specializations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Specializations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SpecializationCourses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "SpecializationCourses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Semesters",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Semesters",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SemesterGPAs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "SemesterGPAs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Registrations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Registrations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RevokedAt",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Fees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Fees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Departments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Departments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "DepartmentCourses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "DepartmentCourses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CourseUploads",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "CourseUploads",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CoursePrerequisites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CoursePrerequisites",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CoursePrerequisites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUserRoles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AspNetUserRoles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetRoles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AspNetRoles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "AcademicSchedules",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admins_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assistants",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assistants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assistants_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assistants_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Instructors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instructors_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Instructors_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AcademicCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Level = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TotalCredits = table.Column<int>(type: "int", nullable: false),
                    AllowedCredits = table.Column<int>(type: "int", nullable: false),
                    TotalGPA = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    SpecializationId = table.Column<int>(type: "int", nullable: true),
                    DepartmentId1 = table.Column<int>(type: "int", nullable: true),
                    SpecializationId1 = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_Departments_DepartmentId1",
                        column: x => x.DepartmentId1,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Students_Specializations_SpecializationId",
                        column: x => x.SpecializationId,
                        principalTable: "Specializations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Students_Specializations_SpecializationId1",
                        column: x => x.SpecializationId1,
                        principalTable: "Specializations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AssistantCourseUploads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssistantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SemesterId = table.Column<int>(type: "int", nullable: false),
                    StudyYearId = table.Column<int>(type: "int", nullable: false),
                    SemesterId1 = table.Column<int>(type: "int", nullable: true),
                    StudyYearId1 = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssistantCourseUploads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssistantCourseUploads_Assistants_AssistantId",
                        column: x => x.AssistantId,
                        principalTable: "Assistants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssistantCourseUploads_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssistantCourseUploads_Semesters_SemesterId1",
                        column: x => x.SemesterId1,
                        principalTable: "Semesters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AssistantCourseUploads_StudyYears_StudyYearId",
                        column: x => x.StudyYearId,
                        principalTable: "StudyYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssistantCourseUploads_StudyYears_StudyYearId1",
                        column: x => x.StudyYearId1,
                        principalTable: "StudyYears",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseAssistants",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    SemesterId = table.Column<int>(type: "int", nullable: false),
                    StudyYearId = table.Column<int>(type: "int", nullable: false),
                    AssistantUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SemesterId1 = table.Column<int>(type: "int", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseAssistants", x => new { x.CourseId, x.AssistantUserId, x.SemesterId, x.StudyYearId });
                    table.ForeignKey(
                        name: "FK_CourseAssistants_Assistants_AssistantUserId",
                        column: x => x.AssistantUserId,
                        principalTable: "Assistants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseAssistants_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseAssistants_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseAssistants_Semesters_SemesterId1",
                        column: x => x.SemesterId1,
                        principalTable: "Semesters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CourseAssistants_StudyYears_StudyYearId",
                        column: x => x.StudyYearId,
                        principalTable: "StudyYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    SemesterId = table.Column<int>(type: "int", nullable: false),
                    StudyYearId = table.Column<int>(type: "int", nullable: false),
                    InstructorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AssistantId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxScore = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    AllowLateSubmission = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                    table.CheckConstraint("CK_Assignment_CreatorExclusive", "(\"InstructorId\" IS NOT NULL AND \"AssistantId\" IS NULL) OR (\"InstructorId\" IS NULL AND \"AssistantId\" IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_Assignments_Assistants_AssistantId",
                        column: x => x.AssistantId,
                        principalTable: "Assistants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignments_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignments_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignments_StudyYears_StudyYearId",
                        column: x => x.StudyYearId,
                        principalTable: "StudyYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseInstructors",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    InstructorUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SemesterId = table.Column<int>(type: "int", nullable: false),
                    StudyYearId = table.Column<int>(type: "int", nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    SemesterId1 = table.Column<int>(type: "int", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseInstructors", x => new { x.CourseId, x.InstructorUserId, x.SemesterId, x.StudyYearId });
                    table.ForeignKey(
                        name: "FK_CourseInstructors_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseInstructors_Instructors_InstructorUserId",
                        column: x => x.InstructorUserId,
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseInstructors_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseInstructors_Semesters_SemesterId1",
                        column: x => x.SemesterId1,
                        principalTable: "Semesters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CourseInstructors_StudyYears_StudyYearId",
                        column: x => x.StudyYearId,
                        principalTable: "StudyYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InstructorAssistant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstructorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssistantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SemesterId = table.Column<int>(type: "int", nullable: false),
                    StudyYearId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorAssistant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstructorAssistant_Assistants_AssistantId",
                        column: x => x.AssistantId,
                        principalTable: "Assistants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstructorAssistant_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstructorAssistant_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstructorAssistant_StudyYears_StudyYearId",
                        column: x => x.StudyYearId,
                        principalTable: "StudyYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    SemesterId = table.Column<int>(type: "int", nullable: false),
                    StudyYearId = table.Column<int>(type: "int", nullable: false),
                    InstructorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AssistantId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    TotalScore = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: false),
                    MaxAttempts = table.Column<int>(type: "int", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.Id);
                    table.CheckConstraint("CK_Quiz_CreatorExclusive", "(\"InstructorId\" IS NOT NULL AND \"AssistantId\" IS NULL) OR (\"InstructorId\" IS NULL AND \"AssistantId\" IS NOT NULL)");
                    table.CheckConstraint("CK_Quiz_EndAfterStart", "\"EndTime\" > \"StartTime\"");
                    table.ForeignKey(
                        name: "FK_Quizzes_Assistants_AssistantId",
                        column: x => x.AssistantId,
                        principalTable: "Assistants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quizzes_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Quizzes_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quizzes_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quizzes_StudyYears_StudyYearId",
                        column: x => x.StudyYearId,
                        principalTable: "StudyYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentStudyYears",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    StudyYearId = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    EnrolledAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentStudyYears", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentStudyYears_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentStudyYears_StudyYears_StudyYearId",
                        column: x => x.StudyYearId,
                        principalTable: "StudyYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssistantCourseUploadItems",
                columns: table => new
                {
                    AssistantCourseUploadId = table.Column<int>(type: "int", nullable: false),
                    CourseUploadId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssistantCourseUploadItems", x => new { x.AssistantCourseUploadId, x.CourseUploadId });
                    table.ForeignKey(
                        name: "FK_AssistantCourseUploadItems_AssistantCourseUploads_AssistantCourseUploadId",
                        column: x => x.AssistantCourseUploadId,
                        principalTable: "AssistantCourseUploads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssistantCourseUploadItems_CourseUploads_CourseUploadId",
                        column: x => x.CourseUploadId,
                        principalTable: "CourseUploads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentSubmissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignmentId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Score = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true),
                    Feedback = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    GradedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GradedByInstructorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    GradedByAssistantId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignmentSubmissions_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentSubmissions_Assistants_GradedByAssistantId",
                        column: x => x.GradedByAssistantId,
                        principalTable: "Assistants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssignmentSubmissions_Instructors_GradedByInstructorId",
                        column: x => x.GradedByInstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssignmentSubmissions_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstructorCourseUploads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstructorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SemesterId = table.Column<int>(type: "int", nullable: false),
                    StudyYearId = table.Column<int>(type: "int", nullable: false),
                    CourseInstructorCourseId = table.Column<int>(type: "int", nullable: true),
                    CourseInstructorInstructorUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CourseInstructorSemesterId = table.Column<int>(type: "int", nullable: true),
                    CourseInstructorStudyYearId = table.Column<int>(type: "int", nullable: true),
                    SemesterId1 = table.Column<int>(type: "int", nullable: true),
                    StudyYearId1 = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorCourseUploads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstructorCourseUploads_CourseInstructors_CourseInstructorCourseId_CourseInstructorInstructorUserId_CourseInstructorSemester~",
                        columns: x => new { x.CourseInstructorCourseId, x.CourseInstructorInstructorUserId, x.CourseInstructorSemesterId, x.CourseInstructorStudyYearId },
                        principalTable: "CourseInstructors",
                        principalColumns: new[] { "CourseId", "InstructorUserId", "SemesterId", "StudyYearId" });
                    table.ForeignKey(
                        name: "FK_InstructorCourseUploads_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstructorCourseUploads_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstructorCourseUploads_Semesters_SemesterId1",
                        column: x => x.SemesterId1,
                        principalTable: "Semesters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InstructorCourseUploads_StudyYears_StudyYearId",
                        column: x => x.StudyYearId,
                        principalTable: "StudyYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstructorCourseUploads_StudyYears_StudyYearId1",
                        column: x => x.StudyYearId1,
                        principalTable: "StudyYears",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuizId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Points = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    CorrectAnswerText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizAttempts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuizId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AttemptNumber = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Score = table.Column<decimal>(type: "decimal(6,2)", precision: 6, scale: 2, nullable: true),
                    IsGraded = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizAttempts_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizAttempts_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstructorCourseUploadItems",
                columns: table => new
                {
                    InstructorCourseUploadId = table.Column<int>(type: "int", nullable: false),
                    CourseUploadId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorCourseUploadItems", x => new { x.InstructorCourseUploadId, x.CourseUploadId });
                    table.ForeignKey(
                        name: "FK_InstructorCourseUploadItems_CourseUploads_CourseUploadId",
                        column: x => x.CourseUploadId,
                        principalTable: "CourseUploads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstructorCourseUploadItems_InstructorCourseUploads_InstructorCourseUploadId",
                        column: x => x.InstructorCourseUploadId,
                        principalTable: "InstructorCourseUploads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionOptions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuizAttemptId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    SelectedOptionId = table.Column<int>(type: "int", nullable: true),
                    AnswerText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: true),
                    PointsAwarded = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentAnswers_QuestionOptions_SelectedOptionId",
                        column: x => x.SelectedOptionId,
                        principalTable: "QuestionOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentAnswers_QuizAttempts_QuizAttemptId",
                        column: x => x.QuizAttemptId,
                        principalTable: "QuizAttempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseUploads_StudentId",
                table: "CourseUploads",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicSchedules_StudentId",
                table: "AcademicSchedules",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssistantId",
                table: "Assignments",
                column: "AssistantId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_CourseId",
                table: "Assignments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_InstructorId",
                table: "Assignments",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_SemesterId",
                table: "Assignments",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_StudyYearId",
                table: "Assignments",
                column: "StudyYearId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentSubmissions_AssignmentId_StudentId",
                table: "AssignmentSubmissions",
                columns: new[] { "AssignmentId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentSubmissions_GradedByAssistantId",
                table: "AssignmentSubmissions",
                column: "GradedByAssistantId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentSubmissions_GradedByInstructorId",
                table: "AssignmentSubmissions",
                column: "GradedByInstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentSubmissions_StudentId",
                table: "AssignmentSubmissions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssistantCourseUploadItems_CourseUploadId",
                table: "AssistantCourseUploadItems",
                column: "CourseUploadId");

            migrationBuilder.CreateIndex(
                name: "IX_AssistantCourseUploads_AssistantId_SemesterId_StudyYearId",
                table: "AssistantCourseUploads",
                columns: new[] { "AssistantId", "SemesterId", "StudyYearId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssistantCourseUploads_SemesterId",
                table: "AssistantCourseUploads",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_AssistantCourseUploads_SemesterId1",
                table: "AssistantCourseUploads",
                column: "SemesterId1");

            migrationBuilder.CreateIndex(
                name: "IX_AssistantCourseUploads_StudyYearId",
                table: "AssistantCourseUploads",
                column: "StudyYearId");

            migrationBuilder.CreateIndex(
                name: "IX_AssistantCourseUploads_StudyYearId1",
                table: "AssistantCourseUploads",
                column: "StudyYearId1");

            migrationBuilder.CreateIndex(
                name: "IX_Assistants_DepartmentId",
                table: "Assistants",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAssistants_AssistantUserId",
                table: "CourseAssistants",
                column: "AssistantUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAssistants_SemesterId",
                table: "CourseAssistants",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAssistants_SemesterId1",
                table: "CourseAssistants",
                column: "SemesterId1");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAssistants_StudyYearId",
                table: "CourseAssistants",
                column: "StudyYearId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseInstructors_InstructorUserId",
                table: "CourseInstructors",
                column: "InstructorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseInstructors_SemesterId",
                table: "CourseInstructors",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseInstructors_SemesterId1",
                table: "CourseInstructors",
                column: "SemesterId1");

            migrationBuilder.CreateIndex(
                name: "IX_CourseInstructors_StudyYearId",
                table: "CourseInstructors",
                column: "StudyYearId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorAssistant_AssistantId",
                table: "InstructorAssistant",
                column: "AssistantId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorAssistant_InstructorId_AssistantId_SemesterId_StudyYearId",
                table: "InstructorAssistant",
                columns: new[] { "InstructorId", "AssistantId", "SemesterId", "StudyYearId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InstructorAssistant_SemesterId",
                table: "InstructorAssistant",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorAssistant_StudyYearId",
                table: "InstructorAssistant",
                column: "StudyYearId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorCourseUploadItems_CourseUploadId",
                table: "InstructorCourseUploadItems",
                column: "CourseUploadId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorCourseUploads_CourseInstructorCourseId_CourseInstructorInstructorUserId_CourseInstructorSemesterId_CourseInstructo~",
                table: "InstructorCourseUploads",
                columns: new[] { "CourseInstructorCourseId", "CourseInstructorInstructorUserId", "CourseInstructorSemesterId", "CourseInstructorStudyYearId" });

            migrationBuilder.CreateIndex(
                name: "IX_InstructorCourseUploads_InstructorId_SemesterId_StudyYearId",
                table: "InstructorCourseUploads",
                columns: new[] { "InstructorId", "SemesterId", "StudyYearId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InstructorCourseUploads_SemesterId",
                table: "InstructorCourseUploads",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorCourseUploads_SemesterId1",
                table: "InstructorCourseUploads",
                column: "SemesterId1");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorCourseUploads_StudyYearId",
                table: "InstructorCourseUploads",
                column: "StudyYearId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorCourseUploads_StudyYearId1",
                table: "InstructorCourseUploads",
                column: "StudyYearId1");

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_DepartmentId",
                table: "Instructors",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_QuestionId",
                table: "QuestionOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizId",
                table: "Questions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_QuizId_StudentId_AttemptNumber",
                table: "QuizAttempts",
                columns: new[] { "QuizId", "StudentId", "AttemptNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_StudentId",
                table: "QuizAttempts",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_AssistantId",
                table: "Quizzes",
                column: "AssistantId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_CourseId",
                table: "Quizzes",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_InstructorId",
                table: "Quizzes",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_SemesterId",
                table: "Quizzes",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_StudyYearId",
                table: "Quizzes",
                column: "StudyYearId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAnswers_QuestionId",
                table: "StudentAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAnswers_QuizAttemptId_QuestionId",
                table: "StudentAnswers",
                columns: new[] { "QuizAttemptId", "QuestionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentAnswers_SelectedOptionId",
                table: "StudentAnswers",
                column: "SelectedOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_AcademicCode",
                table: "Students",
                column: "AcademicCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_DepartmentId",
                table: "Students",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_DepartmentId1",
                table: "Students",
                column: "DepartmentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Students_SpecializationId",
                table: "Students",
                column: "SpecializationId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_SpecializationId1",
                table: "Students",
                column: "SpecializationId1");

            migrationBuilder.CreateIndex(
                name: "IX_StudentStudyYears_StudentId_StudyYearId",
                table: "StudentStudyYears",
                columns: new[] { "StudentId", "StudyYearId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentStudyYears_StudyYearId",
                table: "StudentStudyYears",
                column: "StudyYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicSchedules_Admins_AdminId",
                table: "AcademicSchedules",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicSchedules_Students_StudentId",
                table: "AcademicSchedules",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUploads_Students_StudentId",
                table: "CourseUploads",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_Students_StudentId",
                table: "Registrations",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SemesterGPAs_Students_StudentId",
                table: "SemesterGPAs",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicSchedules_Admins_AdminId",
                table: "AcademicSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_AcademicSchedules_Students_StudentId",
                table: "AcademicSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseUploads_Students_StudentId",
                table: "CourseUploads");

            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Students_StudentId",
                table: "Registrations");

            migrationBuilder.DropForeignKey(
                name: "FK_SemesterGPAs_Students_StudentId",
                table: "SemesterGPAs");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "AssignmentSubmissions");

            migrationBuilder.DropTable(
                name: "AssistantCourseUploadItems");

            migrationBuilder.DropTable(
                name: "CourseAssistants");

            migrationBuilder.DropTable(
                name: "InstructorAssistant");

            migrationBuilder.DropTable(
                name: "InstructorCourseUploadItems");

            migrationBuilder.DropTable(
                name: "StudentAnswers");

            migrationBuilder.DropTable(
                name: "StudentStudyYears");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "AssistantCourseUploads");

            migrationBuilder.DropTable(
                name: "InstructorCourseUploads");

            migrationBuilder.DropTable(
                name: "QuestionOptions");

            migrationBuilder.DropTable(
                name: "QuizAttempts");

            migrationBuilder.DropTable(
                name: "CourseInstructors");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Quizzes");

            migrationBuilder.DropTable(
                name: "Assistants");

            migrationBuilder.DropTable(
                name: "Instructors");

            migrationBuilder.DropIndex(
                name: "IX_CourseUploads_StudentId",
                table: "CourseUploads");

            migrationBuilder.DropIndex(
                name: "IX_AcademicSchedules_StudentId",
                table: "AcademicSchedules");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "StudyYears");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "StudyYears");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Specializations");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Specializations");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SpecializationCourses");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "SpecializationCourses");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Semesters");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Semesters");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SemesterGPAs");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "SemesterGPAs");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "RevokedAt",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Fees");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Fees");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "DepartmentCourses");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "DepartmentCourses");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CourseUploads");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "CourseUploads");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CoursePrerequisites");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CoursePrerequisites");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CoursePrerequisites");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "AcademicSchedules");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "SemesterGPAs",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SemesterGPAs_StudentId_SemesterId_StudyYearId",
                table: "SemesterGPAs",
                newName: "IX_SemesterGPAs_UserId_SemesterId_StudyYearId");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Registrations",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Registrations_StudentId",
                table: "Registrations",
                newName: "IX_Registrations_UserId");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "CourseUploads",
                newName: "UploadedAt");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AspNetUsers",
                newName: "DisplayName");

            migrationBuilder.RenameColumn(
                name: "AdminId",
                table: "AcademicSchedules",
                newName: "UploadedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_AcademicSchedules_AdminId",
                table: "AcademicSchedules",
                newName: "IX_AcademicSchedules_UploadedByUserId");

            migrationBuilder.AddColumn<string>(
                name: "FileId",
                table: "CourseUploads",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UploadedByUserId",
                table: "CourseUploads",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AcademicCode",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AllowedCredits",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpecializationId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalCredits",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalGPA",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileId",
                table: "AcademicSchedules",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserStudyYears",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudyYearId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    EnrolledAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStudyYears", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStudyYears_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStudyYears_StudyYears_StudyYearId",
                        column: x => x.StudyYearId,
                        principalTable: "StudyYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseUploads_UploadedByUserId",
                table: "CourseUploads",
                column: "UploadedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AcademicCode",
                table: "AspNetUsers",
                column: "AcademicCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SpecializationId",
                table: "AspNetUsers",
                column: "SpecializationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStudyYears_StudyYearId",
                table: "UserStudyYears",
                column: "StudyYearId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStudyYears_UserId_StudyYearId",
                table: "UserStudyYears",
                columns: new[] { "UserId", "StudyYearId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicSchedules_AspNetUsers_UploadedByUserId",
                table: "AcademicSchedules",
                column: "UploadedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Departments_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Specializations_SpecializationId",
                table: "AspNetUsers",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUploads_AspNetUsers_UploadedByUserId",
                table: "CourseUploads",
                column: "UploadedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_AspNetUsers_UserId",
                table: "Registrations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SemesterGPAs_AspNetUsers_UserId",
                table: "SemesterGPAs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
