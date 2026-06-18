using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Domain.Enums;
using University_Management_System.Infrastructure.Presistence;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Infrastructure.Presistence.Data
{
    public class DataSeeding : IDataSeeding
    {
        private readonly UniversityDbContext _dbContext;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public DataSeeding(
            UniversityDbContext dbContext,
            RoleManager<Role> roleManager,
            UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedDataInfoAsync()
        {
            try
            {
                var pendingMigration = await _dbContext.Database.GetPendingMigrationsAsync();
                if (pendingMigration.Any())
                    await _dbContext.Database.MigrateAsync();

                // ================= Departments =================
                if (!_dbContext.Departments.Any())
                {
                    var departments = new List<Department>
                    {
                        new() { Name = "Computer Science",   Code = "CS",  HasPreparatoryYear = false },
                        new() { Name = "Business English",   Code = "BE",  HasPreparatoryYear = false },
                        new() { Name = "Business Arabic",    Code = "BA",  HasPreparatoryYear = false },
                        new() { Name = "Journalism",         Code = "JR",  HasPreparatoryYear = false },
                        new() { Name = "Engineering",        Code = "ENG", HasPreparatoryYear = true  }
                    };
                    await _dbContext.Departments.AddRangeAsync(departments);
                    await _dbContext.SaveChangesAsync();
                }

                // ================= Study Years =================
                if (!_dbContext.StudyYears.Any())
                {
                    var studyYears = new List<StudyYear>();
                    for (int year = 2018; year <= 2025; year++)
                    {
                        studyYears.Add(new StudyYear
                        {
                            StartYear = year,
                            EndYear   = year + 1,
                            IsCurrent = year == 2025
                        });
                    }
                    await _dbContext.StudyYears.AddRangeAsync(studyYears);
                    await _dbContext.SaveChangesAsync();
                }

                // ================= Semesters =================
                if (!_dbContext.Semesters.Any())
                {
                    var allStudyYears = await _dbContext.StudyYears.ToListAsync();
                    var semesters = new List<Semester>();
                    foreach (var sy in allStudyYears)
                    {
                        semesters.Add(new Semester
                        {
                            Title       = SemesterEnum.First_Semester,
                            StartDate   = new DateTime(sy.StartYear, 9, 1),
                            EndDate     = new DateTime(sy.StartYear, 12, 31),
                            StudyYearId = sy.Id
                        });
                        semesters.Add(new Semester
                        {
                            Title       = SemesterEnum.Second_Semester,
                            StartDate   = new DateTime(sy.EndYear, 1, 1),
                            EndDate     = new DateTime(sy.EndYear, 5, 31),
                            StudyYearId = sy.Id
                        });
                    }
                    await _dbContext.Semesters.AddRangeAsync(semesters);
                    await _dbContext.SaveChangesAsync();
                }

                // ================= Courses =================
                if (!_dbContext.Courses.Any())
                {
                    var csId  = (await _dbContext.Departments.FirstAsync(d => d.Code == "CS")).Id;
                    var beId  = (await _dbContext.Departments.FirstAsync(d => d.Code == "BE")).Id;
                    var baId  = (await _dbContext.Departments.FirstAsync(d => d.Code == "BA")).Id;
                    var jrId  = (await _dbContext.Departments.FirstAsync(d => d.Code == "JR")).Id;
                    var engId = (await _dbContext.Departments.FirstAsync(d => d.Code == "ENG")).Id;

                    var courses = new List<Course>
                    {
                        // ── Computer Science ────────────────────────────────────────────
                        // Year 1
                        new() { Code="CS101",   Name="Introduction to Computer Science",  Credits=3,  DepartmentId=csId },
                        new() { Code="CS102",   Name="Programming Fundamentals I",         Credits=4,  DepartmentId=csId },
                        new() { Code="CS103",   Name="Programming Fundamentals II",        Credits=4,  DepartmentId=csId },
                        new() { Code="CS104",   Name="Computer Organization",              Credits=3,  DepartmentId=csId },
                        new() { Code="MATH101", Name="Discrete Mathematics",               Credits=3,  DepartmentId=csId },
                        new() { Code="MATH102", Name="Calculus I",                         Credits=3,  DepartmentId=csId },
                        new() { Code="GEN101",  Name="English Communication",              Credits=2,  DepartmentId=csId },
                        new() { Code="PHY101",  Name="Physics I",                          Credits=3,  DepartmentId=csId },
                        // Year 2
                        new() { Code="CS201",   Name="Data Structures",                    Credits=4,  DepartmentId=csId },
                        new() { Code="CS202",   Name="Algorithms",                         Credits=4,  DepartmentId=csId },
                        new() { Code="CS203",   Name="Object-Oriented Programming",        Credits=3,  DepartmentId=csId },
                        new() { Code="CS204",   Name="Database Systems",                   Credits=3,  DepartmentId=csId },
                        new() { Code="CS205",   Name="Web Development",                    Credits=3,  DepartmentId=csId },
                        new() { Code="MATH201", Name="Linear Algebra",                     Credits=3,  DepartmentId=csId },
                        new() { Code="STAT201", Name="Statistics",                         Credits=3,  DepartmentId=csId },
                        new() { Code="GEN201",  Name="Technical Writing",                  Credits=2,  DepartmentId=csId },
                        // Year 3
                        new() { Code="CS301",   Name="Software Engineering",               Credits=4,  DepartmentId=csId },
                        new() { Code="CS302",   Name="Operating Systems",                  Credits=4,  DepartmentId=csId },
                        new() { Code="CS303",   Name="Computer Networks",                  Credits=3,  DepartmentId=csId },
                        new() { Code="CS304",   Name="Artificial Intelligence",            Credits=3,  DepartmentId=csId },
                        new() { Code="CS305",   Name="Machine Learning",                   Credits=3,  DepartmentId=csId },
                        new() { Code="CS306",   Name="Cybersecurity",                      Credits=3,  DepartmentId=csId },
                        new() { Code="CS307",   Name="Mobile App Development",             Credits=3,  DepartmentId=csId },
                        new() { Code="MATH301", Name="Numerical Methods",                  Credits=3,  DepartmentId=csId },
                        // Year 4
                        new() { Code="CS401",   Name="Advanced Algorithms",                Credits=4,  DepartmentId=csId },
                        new() { Code="CS402",   Name="Distributed Systems",                Credits=3,  DepartmentId=csId },
                        new() { Code="CS403",   Name="Computer Graphics",                  Credits=3,  DepartmentId=csId },
                        new() { Code="CS404",   Name="Big Data Analytics",                 Credits=3,  DepartmentId=csId },
                        new() { Code="CS405",   Name="Cloud Computing",                    Credits=3,  DepartmentId=csId },
                        new() { Code="CS406",   Name="Blockchain Technology",              Credits=3,  DepartmentId=csId },
                        new() { Code="CS407",   Name="IoT and Embedded Systems",           Credits=3,  DepartmentId=csId },
                        new() { Code="CS408",   Name="Capstone Project I",                 Credits=4,  DepartmentId=csId },
                        new() { Code="CS409",   Name="Capstone Project II",                Credits=4,  DepartmentId=csId },
                        new() { Code="BUS401",  Name="Entrepreneurship",                   Credits=2,  DepartmentId=csId },
                        // CS Electives
                        new() { Code="CS501",   Name="Advanced Machine Learning",          Credits=3,  DepartmentId=csId },
                        new() { Code="CS502",   Name="Deep Learning",                      Credits=3,  DepartmentId=csId },
                        new() { Code="CS503",   Name="Natural Language Processing",        Credits=3,  DepartmentId=csId },
                        new() { Code="CS504",   Name="Computer Vision",                    Credits=3,  DepartmentId=csId },
                        new() { Code="CS505",   Name="Quantum Computing",                  Credits=3,  DepartmentId=csId },
                        new() { Code="CS506",   Name="Game Development",                   Credits=3,  DepartmentId=csId },
                        new() { Code="CS507",   Name="DevOps",                             Credits=3,  DepartmentId=csId },
                        new() { Code="CS508",   Name="Ethical Hacking",                    Credits=3,  DepartmentId=csId },
                        new() { Code="CS509",   Name="Data Mining",                        Credits=3,  DepartmentId=csId },
                        new() { Code="CS510",   Name="Parallel Computing",                 Credits=3,  DepartmentId=csId },
                        new() { Code="CS511",   Name="Compiler Design",                    Credits=3,  DepartmentId=csId },
                        new() { Code="CS512",   Name="Human-Computer Interaction",         Credits=3,  DepartmentId=csId },
                        new() { Code="CS513",   Name="Software Testing",                   Credits=3,  DepartmentId=csId },
                        new() { Code="CS514",   Name="Cryptography",                       Credits=3,  DepartmentId=csId },
                        new() { Code="CS515",   Name="Augmented Reality",                  Credits=3,  DepartmentId=csId },
                        new() { Code="CS516",   Name="Virtual Reality",                    Credits=3,  DepartmentId=csId },
                        new() { Code="CS517",   Name="Bioinformatics",                     Credits=3,  DepartmentId=csId },
                        new() { Code="CS518",   Name="Robotics",                           Credits=3,  DepartmentId=csId },
                        new() { Code="CS519",   Name="Digital Signal Processing",          Credits=3,  DepartmentId=csId },
                        new() { Code="CS520",   Name="Information Retrieval",              Credits=3,  DepartmentId=csId },
                        new() { Code="CS521",   Name="Network Security",                   Credits=3,  DepartmentId=csId },
                        new() { Code="CS522",   Name="Advanced Databases",                 Credits=3,  DepartmentId=csId },
                        new() { Code="CS523",   Name="Microservices Architecture",         Credits=3,  DepartmentId=csId },
                        new() { Code="CS524",   Name="Serverless Computing",               Credits=3,  DepartmentId=csId },
                        new() { Code="CS525",   Name="Edge Computing",                     Credits=3,  DepartmentId=csId },

                        // ── Business English ─────────────────────────────────────────────
                        // Year 1
                        new() { Code="BE101",   Name="Business English I",                 Credits=3,  DepartmentId=beId },
                        new() { Code="BE102",   Name="Business English II",                Credits=3,  DepartmentId=beId },
                        new() { Code="BE103",   Name="Principles of Management",           Credits=3,  DepartmentId=beId },
                        new() { Code="BE104",   Name="Introduction to Economics",          Credits=3,  DepartmentId=beId },
                        new() { Code="BE105",   Name="Business Mathematics",               Credits=3,  DepartmentId=beId },
                        new() { Code="BE106",   Name="Introduction to Accounting",         Credits=3,  DepartmentId=beId },
                        new() { Code="BE107",   Name="English Academic Writing",           Credits=2,  DepartmentId=beId },
                        new() { Code="BE108",   Name="Computer Applications for Business", Credits=2,  DepartmentId=beId },
                        // Year 2
                        new() { Code="BE201",   Name="Microeconomics",                     Credits=3,  DepartmentId=beId },
                        new() { Code="BE202",   Name="Macroeconomics",                     Credits=3,  DepartmentId=beId },
                        new() { Code="BE203",   Name="Financial Accounting",               Credits=3,  DepartmentId=beId },
                        new() { Code="BE204",   Name="Business Communication",             Credits=3,  DepartmentId=beId },
                        new() { Code="BE205",   Name="Marketing Principles",               Credits=3,  DepartmentId=beId },
                        new() { Code="BE206",   Name="Organizational Behavior",            Credits=3,  DepartmentId=beId },
                        new() { Code="BE207",   Name="Business Statistics",                Credits=3,  DepartmentId=beId },
                        new() { Code="BE208",   Name="Business Law",                       Credits=2,  DepartmentId=beId },
                        // Year 3
                        new() { Code="BE301",   Name="Corporate Finance",                  Credits=3,  DepartmentId=beId },
                        new() { Code="BE302",   Name="Strategic Management",               Credits=3,  DepartmentId=beId },
                        new() { Code="BE303",   Name="International Business",             Credits=3,  DepartmentId=beId },
                        new() { Code="BE304",   Name="Human Resource Management",          Credits=3,  DepartmentId=beId },
                        new() { Code="BE305",   Name="Operations Management",              Credits=3,  DepartmentId=beId },
                        new() { Code="BE306",   Name="Business Research Methods",          Credits=3,  DepartmentId=beId },
                        new() { Code="BE307",   Name="Cross-Cultural Communication",       Credits=3,  DepartmentId=beId },
                        new() { Code="BE308",   Name="Management Information Systems",     Credits=3,  DepartmentId=beId },
                        // Year 4
                        new() { Code="BE401",   Name="Business Strategy",                  Credits=3,  DepartmentId=beId },
                        new() { Code="BE402",   Name="Entrepreneurship and Innovation",    Credits=3,  DepartmentId=beId },
                        new() { Code="BE403",   Name="Investment Analysis",                Credits=3,  DepartmentId=beId },
                        new() { Code="BE404",   Name="Digital Marketing",                  Credits=3,  DepartmentId=beId },
                        new() { Code="BE405",   Name="Supply Chain Management",            Credits=3,  DepartmentId=beId },
                        new() { Code="BE406",   Name="Business Ethics",                    Credits=2,  DepartmentId=beId },
                        new() { Code="BE407",   Name="Graduation Project",                 Credits=6,  DepartmentId=beId },
                        // BE Electives
                        new() { Code="BE501",   Name="E-Commerce",                         Credits=3,  DepartmentId=beId },
                        new() { Code="BE502",   Name="Banking and Financial Institutions", Credits=3,  DepartmentId=beId },
                        new() { Code="BE503",   Name="Project Management",                 Credits=3,  DepartmentId=beId },
                        new() { Code="BE504",   Name="English for Negotiations",           Credits=3,  DepartmentId=beId },
                        new() { Code="BE505",   Name="Business Translation (EN-AR)",       Credits=3,  DepartmentId=beId },

                        // ── Business Arabic ──────────────────────────────────────────────
                        // Year 1
                        new() { Code="BA101",   Name="Arabic Business Communication I",    Credits=3,  DepartmentId=baId },
                        new() { Code="BA102",   Name="Arabic Business Communication II",   Credits=3,  DepartmentId=baId },
                        new() { Code="BA103",   Name="Principles of Management",           Credits=3,  DepartmentId=baId },
                        new() { Code="BA104",   Name="Introduction to Economics",          Credits=3,  DepartmentId=baId },
                        new() { Code="BA105",   Name="Business Mathematics",               Credits=3,  DepartmentId=baId },
                        new() { Code="BA106",   Name="Introduction to Accounting",         Credits=3,  DepartmentId=baId },
                        new() { Code="BA107",   Name="Arabic Academic Writing",            Credits=2,  DepartmentId=baId },
                        new() { Code="BA108",   Name="Computer Applications for Business", Credits=2,  DepartmentId=baId },
                        // Year 2
                        new() { Code="BA201",   Name="Microeconomics",                     Credits=3,  DepartmentId=baId },
                        new() { Code="BA202",   Name="Macroeconomics",                     Credits=3,  DepartmentId=baId },
                        new() { Code="BA203",   Name="Financial Accounting",               Credits=3,  DepartmentId=baId },
                        new() { Code="BA204",   Name="Arabic Business Correspondence",     Credits=3,  DepartmentId=baId },
                        new() { Code="BA205",   Name="Marketing Principles",               Credits=3,  DepartmentId=baId },
                        new() { Code="BA206",   Name="Organizational Behavior",            Credits=3,  DepartmentId=baId },
                        new() { Code="BA207",   Name="Business Statistics",                Credits=3,  DepartmentId=baId },
                        new() { Code="BA208",   Name="Commercial Law (Arabic)",            Credits=2,  DepartmentId=baId },
                        // Year 3
                        new() { Code="BA301",   Name="Corporate Finance",                  Credits=3,  DepartmentId=baId },
                        new() { Code="BA302",   Name="Strategic Management",               Credits=3,  DepartmentId=baId },
                        new() { Code="BA303",   Name="Islamic Finance and Banking",        Credits=3,  DepartmentId=baId },
                        new() { Code="BA304",   Name="Human Resource Management",          Credits=3,  DepartmentId=baId },
                        new() { Code="BA305",   Name="Operations Management",              Credits=3,  DepartmentId=baId },
                        new() { Code="BA306",   Name="Business Research Methods",          Credits=3,  DepartmentId=baId },
                        new() { Code="BA307",   Name="Arabic Media and Public Relations",  Credits=3,  DepartmentId=baId },
                        new() { Code="BA308",   Name="Management Information Systems",     Credits=3,  DepartmentId=baId },
                        // Year 4
                        new() { Code="BA401",   Name="Business Strategy",                  Credits=3,  DepartmentId=baId },
                        new() { Code="BA402",   Name="Entrepreneurship and Innovation",    Credits=3,  DepartmentId=baId },
                        new() { Code="BA403",   Name="Investment Analysis",                Credits=3,  DepartmentId=baId },
                        new() { Code="BA404",   Name="Digital Marketing",                  Credits=3,  DepartmentId=baId },
                        new() { Code="BA405",   Name="Supply Chain Management",            Credits=3,  DepartmentId=baId },
                        new() { Code="BA406",   Name="Business Ethics",                    Credits=2,  DepartmentId=baId },
                        new() { Code="BA407",   Name="Graduation Project",                 Credits=6,  DepartmentId=baId },
                        // BA Electives
                        new() { Code="BA501",   Name="E-Commerce",                         Credits=3,  DepartmentId=baId },
                        new() { Code="BA502",   Name="Banking and Financial Institutions", Credits=3,  DepartmentId=baId },
                        new() { Code="BA503",   Name="Project Management",                 Credits=3,  DepartmentId=baId },
                        new() { Code="BA504",   Name="Arabic Rhetoric for Business",       Credits=3,  DepartmentId=baId },
                        new() { Code="BA505",   Name="Business Translation (AR-EN)",       Credits=3,  DepartmentId=baId },

                        // ── Journalism ───────────────────────────────────────────────────
                        // Year 1
                        new() { Code="JR101",   Name="Introduction to Mass Communication", Credits=3, DepartmentId=jrId },
                        new() { Code="JR102",   Name="News Writing Fundamentals",          Credits=3, DepartmentId=jrId },
                        new() { Code="JR103",   Name="Media History",                      Credits=3, DepartmentId=jrId },
                        new() { Code="JR104",   Name="Introduction to Sociology",          Credits=3, DepartmentId=jrId },
                        new() { Code="JR105",   Name="Arabic Language Skills",             Credits=3, DepartmentId=jrId },
                        new() { Code="JR106",   Name="English for Media",                  Credits=2, DepartmentId=jrId },
                        new() { Code="JR107",   Name="Computer Skills for Journalism",     Credits=2, DepartmentId=jrId },
                        new() { Code="JR108",   Name="Media Law and Ethics",               Credits=3, DepartmentId=jrId },
                        // Year 2
                        new() { Code="JR201",   Name="Print Journalism",                   Credits=3, DepartmentId=jrId },
                        new() { Code="JR202",   Name="Broadcast Journalism",               Credits=3, DepartmentId=jrId },
                        new() { Code="JR203",   Name="Photojournalism",                    Credits=3, DepartmentId=jrId },
                        new() { Code="JR204",   Name="Media Research Methods",             Credits=3, DepartmentId=jrId },
                        new() { Code="JR205",   Name="Public Relations",                   Credits=3, DepartmentId=jrId },
                        new() { Code="JR206",   Name="Advertising Principles",             Credits=3, DepartmentId=jrId },
                        new() { Code="JR207",   Name="Political Communication",            Credits=3, DepartmentId=jrId },
                        new() { Code="JR208",   Name="Introduction to TV Production",      Credits=3, DepartmentId=jrId },
                        // Year 3
                        new() { Code="JR301",   Name="Investigative Journalism",           Credits=3, DepartmentId=jrId },
                        new() { Code="JR302",   Name="Digital Journalism",                 Credits=3, DepartmentId=jrId },
                        new() { Code="JR303",   Name="Social Media Journalism",            Credits=3, DepartmentId=jrId },
                        new() { Code="JR304",   Name="Multimedia Storytelling",            Credits=3, DepartmentId=jrId },
                        new() { Code="JR305",   Name="Media Management",                   Credits=3, DepartmentId=jrId },
                        new() { Code="JR306",   Name="Development Communication",          Credits=3, DepartmentId=jrId },
                        new() { Code="JR307",   Name="Radio Production",                   Credits=3, DepartmentId=jrId },
                        new() { Code="JR308",   Name="Documentary Filmmaking",             Credits=3, DepartmentId=jrId },
                        // Year 4
                        new() { Code="JR401",   Name="Advanced Reporting",                 Credits=3, DepartmentId=jrId },
                        new() { Code="JR402",   Name="Crisis Communication",               Credits=3, DepartmentId=jrId },
                        new() { Code="JR403",   Name="Media Economics",                    Credits=3, DepartmentId=jrId },
                        new() { Code="JR404",   Name="Data Journalism",                    Credits=3, DepartmentId=jrId },
                        new() { Code="JR405",   Name="International Journalism",           Credits=3, DepartmentId=jrId },
                        new() { Code="JR406",   Name="Journalism Internship",              Credits=3, DepartmentId=jrId },
                        new() { Code="JR407",   Name="Graduation Project",                 Credits=6, DepartmentId=jrId },
                        // JR Electives
                        new() { Code="JR501",   Name="Sports Journalism",                  Credits=3, DepartmentId=jrId },
                        new() { Code="JR502",   Name="Science and Health Journalism",      Credits=3, DepartmentId=jrId },
                        new() { Code="JR503",   Name="Economic Journalism",                Credits=3, DepartmentId=jrId },
                        new() { Code="JR504",   Name="Media Criticism",                    Credits=3, DepartmentId=jrId },
                        new() { Code="JR505",   Name="AI Tools for Journalism",            Credits=3, DepartmentId=jrId },

                        // ── Engineering (General / Preparatory + Civil specialization) ──
                        // Preparatory Year (Year 0) — shared by all engineering students
                        new() { Code="ENG001",  Name="Engineering Mathematics I",          Credits=4, DepartmentId=engId },
                        new() { Code="ENG002",  Name="Engineering Mathematics II",         Credits=4, DepartmentId=engId },
                        new() { Code="ENG003",  Name="Engineering Physics",                Credits=4, DepartmentId=engId },
                        new() { Code="ENG004",  Name="Engineering Chemistry",              Credits=3, DepartmentId=engId },
                        new() { Code="ENG005",  Name="Engineering Drawing",                Credits=3, DepartmentId=engId },
                        new() { Code="ENG006",  Name="Introduction to Engineering",        Credits=2, DepartmentId=engId },
                        new() { Code="ENG007",  Name="English for Engineers",              Credits=2, DepartmentId=engId },
                        new() { Code="ENG008",  Name="Computer Programming for Engineers", Credits=3, DepartmentId=engId },
                        // Year 1 (Common core)
                        new() { Code="ENG101",  Name="Engineering Mechanics (Statics)",    Credits=3, DepartmentId=engId },
                        new() { Code="ENG102",  Name="Engineering Mechanics (Dynamics)",   Credits=3, DepartmentId=engId },
                        new() { Code="ENG103",  Name="Electrical Circuits",                Credits=3, DepartmentId=engId },
                        new() { Code="ENG104",  Name="Engineering Materials",              Credits=3, DepartmentId=engId },
                        new() { Code="ENG105",  Name="Thermodynamics",                     Credits=3, DepartmentId=engId },
                        new() { Code="ENG106",  Name="Technical Communication",            Credits=2, DepartmentId=engId },
                        new() { Code="ENG107",  Name="Workshop Practice",                  Credits=2, DepartmentId=engId },
                        // Year 2 (Common)
                        new() { Code="ENG201",  Name="Fluid Mechanics",                    Credits=3, DepartmentId=engId },
                        new() { Code="ENG202",  Name="Strength of Materials",              Credits=3, DepartmentId=engId },
                        new() { Code="ENG203",  Name="Structural Analysis I",              Credits=3, DepartmentId=engId },
                        new() { Code="ENG204",  Name="Electronics Fundamentals",           Credits=3, DepartmentId=engId },
                        new() { Code="ENG205",  Name="Numerical Methods for Engineers",    Credits=3, DepartmentId=engId },
                        new() { Code="ENG206",  Name="Engineering Economy",                Credits=2, DepartmentId=engId },
                        new() { Code="ENG207",  Name="Probability and Statistics",         Credits=3, DepartmentId=engId },
                        // Year 3 – Civil Engineering specialization
                        new() { Code="ENG301",  Name="Structural Analysis II",             Credits=3, DepartmentId=engId },
                        new() { Code="ENG302",  Name="Reinforced Concrete Design",         Credits=3, DepartmentId=engId },
                        new() { Code="ENG303",  Name="Geotechnical Engineering",           Credits=3, DepartmentId=engId },
                        new() { Code="ENG304",  Name="Transportation Engineering",         Credits=3, DepartmentId=engId },
                        new() { Code="ENG305",  Name="Hydraulics and Hydrology",           Credits=3, DepartmentId=engId },
                        new() { Code="ENG306",  Name="Construction Management",            Credits=3, DepartmentId=engId },
                        new() { Code="ENG307",  Name="Surveying",                          Credits=3, DepartmentId=engId },
                        // Year 3 – Electrical Engineering specialization
                        new() { Code="ENG311",  Name="Power Systems I",                    Credits=3, DepartmentId=engId },
                        new() { Code="ENG312",  Name="Signals and Systems",                Credits=3, DepartmentId=engId },
                        new() { Code="ENG313",  Name="Digital Electronics",                Credits=3, DepartmentId=engId },
                        new() { Code="ENG314",  Name="Electromagnetic Fields",             Credits=3, DepartmentId=engId },
                        new() { Code="ENG315",  Name="Control Systems",                    Credits=3, DepartmentId=engId },
                        new() { Code="ENG316",  Name="Microprocessors",                    Credits=3, DepartmentId=engId },
                        new() { Code="ENG317",  Name="Communications Theory",              Credits=3, DepartmentId=engId },
                        // Year 4 – Civil
                        new() { Code="ENG401",  Name="Steel Structures",                   Credits=3, DepartmentId=engId },
                        new() { Code="ENG402",  Name="Foundation Engineering",             Credits=3, DepartmentId=engId },
                        new() { Code="ENG403",  Name="Environmental Engineering",          Credits=3, DepartmentId=engId },
                        new() { Code="ENG404",  Name="Bridge Engineering",                 Credits=3, DepartmentId=engId },
                        new() { Code="ENG405",  Name="Project Planning and Scheduling",    Credits=3, DepartmentId=engId },
                        new() { Code="ENG408",  Name="Graduation Project I (Civil)",       Credits=4, DepartmentId=engId },
                        new() { Code="ENG409",  Name="Graduation Project II (Civil)",      Credits=4, DepartmentId=engId },
                        // Year 4 – Electrical
                        new() { Code="ENG411",  Name="Power Systems II",                   Credits=3, DepartmentId=engId },
                        new() { Code="ENG412",  Name="Digital Signal Processing",          Credits=3, DepartmentId=engId },
                        new() { Code="ENG413",  Name="Wireless Communications",            Credits=3, DepartmentId=engId },
                        new() { Code="ENG414",  Name="Renewable Energy Systems",           Credits=3, DepartmentId=engId },
                        new() { Code="ENG415",  Name="VLSI Design",                        Credits=3, DepartmentId=engId },
                        new() { Code="ENG418",  Name="Graduation Project I (Electrical)",  Credits=4, DepartmentId=engId },
                        new() { Code="ENG419",  Name="Graduation Project II (Electrical)", Credits=4, DepartmentId=engId },
                        // Engineering Electives (shared pool)
                        new() { Code="ENG501",  Name="GIS and Remote Sensing",             Credits=3, DepartmentId=engId },
                        new() { Code="ENG502",  Name="Smart Grid Technology",              Credits=3, DepartmentId=engId },
                        new() { Code="ENG503",  Name="Earthquake Engineering",             Credits=3, DepartmentId=engId },
                        new() { Code="ENG504",  Name="Internet of Things (IoT)",           Credits=3, DepartmentId=engId },
                        new() { Code="ENG505",  Name="Engineering Ethics and Society",     Credits=2, DepartmentId=engId },
                    };

                    await _dbContext.Courses.AddRangeAsync(courses);
                    await _dbContext.SaveChangesAsync();
                }

                // ================= Course Prerequisites =================
                if (!_dbContext.CoursePrerequisites.Any())
                {
                    // Load all courses as a code→id dictionary
                    var allCourses = await _dbContext.Courses.ToDictionaryAsync(c => c.Code, c => c.Id);

                    var prereqDefs = new[]
                    {
                        // CS
                        ("CS103","CS102"), ("CS201","CS103"), ("CS202","CS201"),
                        ("CS203","CS102"), ("CS204","CS103"), ("CS401","CS202"),
                        ("CS301","CS201"), ("CS302","CS201"), ("CS303","CS201"),
                        ("CS304","CS201"), ("CS305","CS304"), ("CS306","CS201"),
                        ("CS408","CS301"), ("CS408","CS302"), ("CS409","CS408"),
                        ("CS501","CS305"), ("CS502","CS501"), ("CS503","CS305"),
                        ("CS504","CS305"), ("CS508","CS306"), ("CS521","CS306"),
                        ("CS522","CS204"),
                        // BE
                        ("BE201","BE104"), ("BE202","BE201"), ("BE203","BE106"),
                        ("BE301","BE203"), ("BE302","BE103"), ("BE303","BE302"),
                        ("BE401","BE302"), ("BE403","BE301"),
                        // BA
                        ("BA201","BA104"), ("BA202","BA201"), ("BA203","BA106"),
                        ("BA301","BA203"), ("BA302","BA103"), ("BA303","BA301"),
                        ("BA401","BA302"), ("BA403","BA301"),
                        // JR
                        ("JR201","JR101"), ("JR202","JR101"), ("JR301","JR201"),
                        ("JR302","JR202"), ("JR303","JR302"), ("JR401","JR301"),
                        ("JR404","JR204"),
                        // ENG
                        ("ENG101","ENG001"), ("ENG102","ENG001"), ("ENG103","ENG003"),
                        ("ENG201","ENG101"), ("ENG202","ENG101"), ("ENG203","ENG202"),
                        ("ENG301","ENG203"), ("ENG302","ENG301"), ("ENG303","ENG202"),
                        ("ENG401","ENG302"), ("ENG402","ENG303"),
                        ("ENG311","ENG103"), ("ENG312","ENG103"), ("ENG315","ENG312"),
                        ("ENG411","ENG311"), ("ENG412","ENG312"),
                    };

                    var prerequisites = new List<CoursePrerequisite>();
                    foreach (var (course, prereq) in prereqDefs)
                    {
                        if (allCourses.TryGetValue(course, out var cId) && allCourses.TryGetValue(prereq, out var pId))
                            prerequisites.Add(new CoursePrerequisite { CourseId = cId, PrerequisiteCourseId = pId });
                    }

                    if (prerequisites.Any())
                    {
                        await _dbContext.CoursePrerequisites.AddRangeAsync(prerequisites);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                // ================= Department Courses (DepartmentCourse) =================
                if (!_dbContext.DepartmentCourses.Any())
                {
                    var depts = await _dbContext.Departments.ToDictionaryAsync(d => d.Code, d => d.Id);
                    var allCourses = await _dbContext.Courses
                        .Select(c => new { c.Code, c.Id, c.DepartmentId })
                        .ToListAsync();

                    // Key = "DEPTCODE|COURSECODE" — collision-proof across departments
                    var deptById = depts.ToDictionary(kv => kv.Value, kv => kv.Key);
                    var courseMap = allCourses.ToDictionary(
                        c => $"{(deptById.TryGetValue(c.DepartmentId, out var dk) ? dk : "??")}|{c.Code}",
                        c => c.Id
                    );

                    // Safe helper — logs and skips if key is missing (never throws)
                    DepartmentCourse? DC(string deptCode, string courseCode, CourseRole role)
                    {
                        if (!depts.TryGetValue(deptCode, out var deptId))
                        {
                            Console.WriteLine($"[DC skip] dept not found: {deptCode}");
                            return null;
                        }
                        var key = $"{deptCode}|{courseCode}";
                        if (!courseMap.TryGetValue(key, out var courseId))
                        {
                            Console.WriteLine($"[DC skip] course not found: {key}");
                            return null;
                        }
                        return new DepartmentCourse { DepartmentId = deptId, CourseId = courseId, Role = role };
                    }

                    var dcs = new List<DepartmentCourse?>
                    {
                        // ── CS Department Courses ─────────────────────────────────────────
                        // Major (core required by CS dept)
                        DC("CS","CS101", CourseRole.Major), DC("CS","CS102", CourseRole.Major),
                        DC("CS","CS103", CourseRole.Major), DC("CS","CS104", CourseRole.Major),
                        DC("CS","MATH101",CourseRole.Major),DC("CS","MATH102",CourseRole.Major),
                        DC("CS","GEN101", CourseRole.Major),DC("CS","PHY101", CourseRole.Major),
                        DC("CS","CS201",  CourseRole.Major),DC("CS","CS202",  CourseRole.Major),
                        DC("CS","CS203",  CourseRole.Major),DC("CS","CS204",  CourseRole.Major),
                        DC("CS","CS205",  CourseRole.Major),DC("CS","MATH201",CourseRole.Major),
                        DC("CS","STAT201", CourseRole.Major),DC("CS","GEN201",CourseRole.Major),
                        DC("CS","CS301",  CourseRole.Major),DC("CS","CS302",  CourseRole.Major),
                        DC("CS","CS303",  CourseRole.Major),DC("CS","MATH301",CourseRole.Major),
                        DC("CS","CS401",  CourseRole.Major),DC("CS","CS408",  CourseRole.Major),
                        DC("CS","CS409",  CourseRole.Major),
                        // Minor (technical elective pool shared with other depts)
                        DC("CS","CS304",  CourseRole.Minor), DC("CS","CS305",  CourseRole.Minor),
                        DC("CS","CS306",  CourseRole.Minor), DC("CS","CS307",  CourseRole.Minor),
                        DC("CS","CS402",  CourseRole.Minor), DC("CS","CS403",  CourseRole.Minor),
                        DC("CS","CS404",  CourseRole.Minor), DC("CS","CS405",  CourseRole.Minor),
                        DC("CS","CS406",  CourseRole.Minor), DC("CS","CS407",  CourseRole.Minor),
                        DC("CS","BUS401", CourseRole.Minor),
                        // Elective
                        DC("CS","CS501",CourseRole.Elective),DC("CS","CS502",CourseRole.Elective),
                        DC("CS","CS503",CourseRole.Elective),DC("CS","CS504",CourseRole.Elective),
                        DC("CS","CS505",CourseRole.Elective),DC("CS","CS506",CourseRole.Elective),
                        DC("CS","CS507",CourseRole.Elective),DC("CS","CS508",CourseRole.Elective),
                        DC("CS","CS509",CourseRole.Elective),DC("CS","CS510",CourseRole.Elective),
                        DC("CS","CS511",CourseRole.Elective),DC("CS","CS512",CourseRole.Elective),
                        DC("CS","CS513",CourseRole.Elective),DC("CS","CS514",CourseRole.Elective),
                        DC("CS","CS515",CourseRole.Elective),DC("CS","CS516",CourseRole.Elective),
                        DC("CS","CS517",CourseRole.Elective),DC("CS","CS518",CourseRole.Elective),
                        DC("CS","CS519",CourseRole.Elective),DC("CS","CS520",CourseRole.Elective),
                        DC("CS","CS521",CourseRole.Elective),DC("CS","CS522",CourseRole.Elective),
                        DC("CS","CS523",CourseRole.Elective),DC("CS","CS524",CourseRole.Elective),
                        DC("CS","CS525",CourseRole.Elective),

                        // ── BE Department Courses ─────────────────────────────────────────
                        DC("BE","BE101",CourseRole.Major), DC("BE","BE102",CourseRole.Major),
                        DC("BE","BE103",CourseRole.Major), DC("BE","BE104",CourseRole.Major),
                        DC("BE","BE105",CourseRole.Major), DC("BE","BE106",CourseRole.Major),
                        DC("BE","BE107",CourseRole.Major), DC("BE","BE108",CourseRole.Major),
                        DC("BE","BE201",CourseRole.Major), DC("BE","BE202",CourseRole.Major),
                        DC("BE","BE203",CourseRole.Major), DC("BE","BE204",CourseRole.Major),
                        DC("BE","BE205",CourseRole.Major), DC("BE","BE206",CourseRole.Major),
                        DC("BE","BE207",CourseRole.Major), DC("BE","BE208",CourseRole.Major),
                        DC("BE","BE301",CourseRole.Major), DC("BE","BE302",CourseRole.Major),
                        DC("BE","BE303",CourseRole.Major), DC("BE","BE304",CourseRole.Major),
                        DC("BE","BE305",CourseRole.Major), DC("BE","BE306",CourseRole.Major),
                        DC("BE","BE307",CourseRole.Major), DC("BE","BE308",CourseRole.Major),
                        DC("BE","BE401",CourseRole.Major), DC("BE","BE402",CourseRole.Major),
                        DC("BE","BE403",CourseRole.Major), DC("BE","BE406",CourseRole.Major),
                        DC("BE","BE407",CourseRole.Major),
                        DC("BE","BE404",CourseRole.Minor), DC("BE","BE405",CourseRole.Minor),
                        DC("BE","BE501",CourseRole.Elective), DC("BE","BE502",CourseRole.Elective),
                        DC("BE","BE503",CourseRole.Elective), DC("BE","BE504",CourseRole.Elective),
                        DC("BE","BE505",CourseRole.Elective),

                        // ── BA Department Courses ─────────────────────────────────────────
                        DC("BA","BA101",CourseRole.Major), DC("BA","BA102",CourseRole.Major),
                        DC("BA","BA103",CourseRole.Major), DC("BA","BA104",CourseRole.Major),
                        DC("BA","BA105",CourseRole.Major), DC("BA","BA106",CourseRole.Major),
                        DC("BA","BA107",CourseRole.Major), DC("BA","BA108",CourseRole.Major),
                        DC("BA","BA201",CourseRole.Major), DC("BA","BA202",CourseRole.Major),
                        DC("BA","BA203",CourseRole.Major), DC("BA","BA204",CourseRole.Major),
                        DC("BA","BA205",CourseRole.Major), DC("BA","BA206",CourseRole.Major),
                        DC("BA","BA207",CourseRole.Major), DC("BA","BA208",CourseRole.Major),
                        DC("BA","BA301",CourseRole.Major), DC("BA","BA302",CourseRole.Major),
                        DC("BA","BA303",CourseRole.Major), DC("BA","BA304",CourseRole.Major),
                        DC("BA","BA305",CourseRole.Major), DC("BA","BA306",CourseRole.Major),
                        DC("BA","BA307",CourseRole.Major), DC("BA","BA308",CourseRole.Major),
                        DC("BA","BA401",CourseRole.Major), DC("BA","BA402",CourseRole.Major),
                        DC("BA","BA403",CourseRole.Major), DC("BA","BA406",CourseRole.Major),
                        DC("BA","BA407",CourseRole.Major),
                        DC("BA","BA404",CourseRole.Minor), DC("BA","BA405",CourseRole.Minor),
                        DC("BA","BA501",CourseRole.Elective), DC("BA","BA502",CourseRole.Elective),
                        DC("BA","BA503",CourseRole.Elective), DC("BA","BA504",CourseRole.Elective),
                        DC("BA","BA505",CourseRole.Elective),

                        // ── JR Department Courses ─────────────────────────────────────────
                        DC("JR","JR101",CourseRole.Major), DC("JR","JR102",CourseRole.Major),
                        DC("JR","JR103",CourseRole.Major), DC("JR","JR104",CourseRole.Major),
                        DC("JR","JR105",CourseRole.Major), DC("JR","JR106",CourseRole.Major),
                        DC("JR","JR107",CourseRole.Major), DC("JR","JR108",CourseRole.Major),
                        DC("JR","JR201",CourseRole.Major), DC("JR","JR202",CourseRole.Major),
                        DC("JR","JR203",CourseRole.Major), DC("JR","JR204",CourseRole.Major),
                        DC("JR","JR205",CourseRole.Major), DC("JR","JR206",CourseRole.Major),
                        DC("JR","JR207",CourseRole.Major), DC("JR","JR208",CourseRole.Major),
                        DC("JR","JR301",CourseRole.Major), DC("JR","JR302",CourseRole.Major),
                        DC("JR","JR303",CourseRole.Major), DC("JR","JR304",CourseRole.Major),
                        DC("JR","JR305",CourseRole.Major), DC("JR","JR306",CourseRole.Major),
                        DC("JR","JR307",CourseRole.Major), DC("JR","JR308",CourseRole.Major),
                        DC("JR","JR401",CourseRole.Major), DC("JR","JR402",CourseRole.Major),
                        DC("JR","JR403",CourseRole.Major), DC("JR","JR406",CourseRole.Major),
                        DC("JR","JR407",CourseRole.Major),
                        DC("JR","JR404",CourseRole.Minor), DC("JR","JR405",CourseRole.Minor),
                        DC("JR","JR501",CourseRole.Elective), DC("JR","JR502",CourseRole.Elective),
                        DC("JR","JR503",CourseRole.Elective), DC("JR","JR504",CourseRole.Elective),
                        DC("JR","JR505",CourseRole.Elective),

                        // ── ENG Department Courses ────────────────────────────────────────
                        // Preparatory year = Major for all
                        DC("ENG","ENG001",CourseRole.Major), DC("ENG","ENG002",CourseRole.Major),
                        DC("ENG","ENG003",CourseRole.Major), DC("ENG","ENG004",CourseRole.Major),
                        DC("ENG","ENG005",CourseRole.Major), DC("ENG","ENG006",CourseRole.Major),
                        DC("ENG","ENG007",CourseRole.Major), DC("ENG","ENG008",CourseRole.Major),
                        // Common Years 1-2 = Major
                        DC("ENG","ENG101",CourseRole.Major), DC("ENG","ENG102",CourseRole.Major),
                        DC("ENG","ENG103",CourseRole.Major), DC("ENG","ENG104",CourseRole.Major),
                        DC("ENG","ENG105",CourseRole.Major), DC("ENG","ENG106",CourseRole.Major),
                        DC("ENG","ENG107",CourseRole.Major), DC("ENG","ENG201",CourseRole.Major),
                        DC("ENG","ENG202",CourseRole.Major), DC("ENG","ENG203",CourseRole.Major),
                        DC("ENG","ENG204",CourseRole.Major), DC("ENG","ENG205",CourseRole.Major),
                        DC("ENG","ENG206",CourseRole.Major), DC("ENG","ENG207",CourseRole.Major),
                        // Civil specialization = Major (for Civil) / Minor for rest
                        DC("ENG","ENG301",CourseRole.Major), DC("ENG","ENG302",CourseRole.Major),
                        DC("ENG","ENG303",CourseRole.Major), DC("ENG","ENG304",CourseRole.Major),
                        DC("ENG","ENG305",CourseRole.Major), DC("ENG","ENG306",CourseRole.Major),
                        DC("ENG","ENG307",CourseRole.Major),
                        DC("ENG","ENG401",CourseRole.Major), DC("ENG","ENG402",CourseRole.Major),
                        DC("ENG","ENG403",CourseRole.Major), DC("ENG","ENG404",CourseRole.Major),
                        DC("ENG","ENG405",CourseRole.Major),
                        DC("ENG","ENG408",CourseRole.Major), DC("ENG","ENG409",CourseRole.Major),
                        // Electrical specialization = Minor from Civil perspective
                        DC("ENG","ENG311",CourseRole.Minor), DC("ENG","ENG312",CourseRole.Minor),
                        DC("ENG","ENG313",CourseRole.Minor), DC("ENG","ENG314",CourseRole.Minor),
                        DC("ENG","ENG315",CourseRole.Minor), DC("ENG","ENG316",CourseRole.Minor),
                        DC("ENG","ENG317",CourseRole.Minor),
                        DC("ENG","ENG411",CourseRole.Minor), DC("ENG","ENG412",CourseRole.Minor),
                        DC("ENG","ENG413",CourseRole.Minor), DC("ENG","ENG414",CourseRole.Minor),
                        DC("ENG","ENG415",CourseRole.Minor),
                        DC("ENG","ENG418",CourseRole.Minor), DC("ENG","ENG419",CourseRole.Minor),
                        // Shared electives
                        DC("ENG","ENG501",CourseRole.Elective), DC("ENG","ENG502",CourseRole.Elective),
                        DC("ENG","ENG503",CourseRole.Elective), DC("ENG","ENG504",CourseRole.Elective),
                        DC("ENG","ENG505",CourseRole.Elective),
                    };

                    var validDcs = dcs.Where(d => d != null).Cast<DepartmentCourse>().ToList();
                    if (validDcs.Any())
                    {
                        await _dbContext.DepartmentCourses.AddRangeAsync(validDcs);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                // ================= Specializations =================
                if (!_dbContext.Specializations.Any())
                {
                    var depts = await _dbContext.Departments.ToDictionaryAsync(d => d.Code, d => d.Id);

                    var specs = new List<Specialization>
                    {
                        // CS
                        new() { Name="Artificial Intelligence & Data Science", Description="AI, ML, deep learning and big data pipelines.", DepartmentId=depts["CS"] },
                        new() { Name="Cybersecurity",                          Description="Network security, ethical hacking and cryptography.", DepartmentId=depts["CS"] },
                        new() { Name="Software Engineering",                   Description="Software design, architecture and DevOps.", DepartmentId=depts["CS"] },
                        // BE
                        new() { Name="International Business",                 Description="Global trade, cross-cultural management and finance.", DepartmentId=depts["BE"] },
                        new() { Name="Marketing",                              Description="Consumer behaviour, digital marketing and brand strategy.", DepartmentId=depts["BE"] },
                        new() { Name="Finance",                                Description="Corporate finance, investment and banking.", DepartmentId=depts["BE"] },
                        // BA
                        new() { Name="Islamic Finance",                        Description="Sharia-compliant banking and financial instruments.", DepartmentId=depts["BA"] },
                        new() { Name="Marketing (Arabic)",                     Description="Arabic-market consumer behaviour and media.", DepartmentId=depts["BA"] },
                        new() { Name="Management (Arabic)",                    Description="Arabic-language management and organizational studies.", DepartmentId=depts["BA"] },
                        // JR
                        new() { Name="Digital and Multimedia Journalism",      Description="Online news, social media and multimedia storytelling.", DepartmentId=depts["JR"] },
                        new() { Name="Broadcast Journalism",                   Description="TV, radio production and presenting.", DepartmentId=depts["JR"] },
                        new() { Name="Public Relations and Advertising",       Description="PR strategy, advertising campaigns and crisis communication.", DepartmentId=depts["JR"] },
                        // ENG
                        new() { Name="Civil Engineering",                      Description="Structural, geotechnical and construction engineering.", DepartmentId=depts["ENG"] },
                        new() { Name="Electrical Engineering",                 Description="Power systems, electronics and communications.", DepartmentId=depts["ENG"] },
                    };

                    await _dbContext.Specializations.AddRangeAsync(specs);
                    await _dbContext.SaveChangesAsync();
                }

                // ================= Specialization Courses =================
                if (!_dbContext.SpecializationCourses.Any())
                {
                    var depts2     = await _dbContext.Departments.ToDictionaryAsync(d => d.Code, d => d.Id);
                    var deptById2  = depts2.ToDictionary(kv => kv.Value, kv => kv.Key);
                    var allCoursesList = await _dbContext.Courses
                        .Select(c => new { c.Code, c.Id, c.DepartmentId })
                        .ToListAsync();
                    // Key = "DEPTCODE|COURSECODE" — same collision-proof strategy
                    var courseMap2 = allCoursesList.ToDictionary(
                        c => $"{(deptById2.TryGetValue(c.DepartmentId, out var dk) ? dk : "??")}|{c.Code}",
                        c => c.Id
                    );
                    // Also build a plain code→id map for cross-dept lookups (courses with unique codes)
                    var plainCodeMap = allCoursesList
                        .GroupBy(c => c.Code)
                        .Where(g => g.Count() == 1)
                        .ToDictionary(g => g.Key, g => g.First().Id);

                    var allSpecs = await _dbContext.Specializations.ToDictionaryAsync(s => s.Name, s => s.Id);

                    // Safe helper — tries plain code first, then qualified key; logs and skips if missing
                    SpecializationCourse? SC(string specName, string courseCode, SpecializationCourseRole role)
                    {
                        if (!allSpecs.TryGetValue(specName, out var specId))
                        {
                            Console.WriteLine($"[SC skip] spec not found: {specName}");
                            return null;
                        }
                        if (!plainCodeMap.TryGetValue(courseCode, out var courseId))
                        {
                            Console.WriteLine($"[SC skip] course not found: {courseCode}");
                            return null;
                        }
                        return new SpecializationCourse { SpecializationId = specId, CourseId = courseId, Role = role };
                    }

                    var scs = new List<SpecializationCourse?>
                    {
                        // ── AI & Data Science ─────────────────────────────────────────────
                        // Core (required for every CS student before specialising)
                        SC("Artificial Intelligence & Data Science","CS101", SpecializationCourseRole.Core),
                        SC("Artificial Intelligence & Data Science","CS102", SpecializationCourseRole.Core),
                        SC("Artificial Intelligence & Data Science","CS103", SpecializationCourseRole.Core),
                        SC("Artificial Intelligence & Data Science","CS201", SpecializationCourseRole.Core),
                        SC("Artificial Intelligence & Data Science","CS202", SpecializationCourseRole.Core),
                        SC("Artificial Intelligence & Data Science","MATH101",SpecializationCourseRole.Core),
                        SC("Artificial Intelligence & Data Science","MATH102",SpecializationCourseRole.Core),
                        SC("Artificial Intelligence & Data Science","STAT201",SpecializationCourseRole.Core),
                        // Specialization Core
                        SC("Artificial Intelligence & Data Science","CS304", SpecializationCourseRole.Specialization_Core),
                        SC("Artificial Intelligence & Data Science","CS305", SpecializationCourseRole.Specialization_Core),
                        SC("Artificial Intelligence & Data Science","CS404", SpecializationCourseRole.Specialization_Core),
                        SC("Artificial Intelligence & Data Science","CS501", SpecializationCourseRole.Specialization_Core),
                        SC("Artificial Intelligence & Data Science","CS502", SpecializationCourseRole.Specialization_Core),
                        SC("Artificial Intelligence & Data Science","CS509", SpecializationCourseRole.Specialization_Core),
                        // Electives
                        SC("Artificial Intelligence & Data Science","CS503", SpecializationCourseRole.Elective),
                        SC("Artificial Intelligence & Data Science","CS504", SpecializationCourseRole.Elective),
                        SC("Artificial Intelligence & Data Science","CS517", SpecializationCourseRole.Elective),
                        SC("Artificial Intelligence & Data Science","CS519", SpecializationCourseRole.Elective),
                        SC("Artificial Intelligence & Data Science","CS520", SpecializationCourseRole.Elective),

                        // ── Cybersecurity ────────────────────────────────────────────────
                        SC("Cybersecurity","CS101",  SpecializationCourseRole.Core),
                        SC("Cybersecurity","CS102",  SpecializationCourseRole.Core),
                        SC("Cybersecurity","CS103",  SpecializationCourseRole.Core),
                        SC("Cybersecurity","CS201",  SpecializationCourseRole.Core),
                        SC("Cybersecurity","CS203",  SpecializationCourseRole.Core),
                        SC("Cybersecurity","CS303",  SpecializationCourseRole.Core),
                        SC("Cybersecurity","MATH101",SpecializationCourseRole.Core),
                        SC("Cybersecurity","CS306",  SpecializationCourseRole.Specialization_Core),
                        SC("Cybersecurity","CS508",  SpecializationCourseRole.Specialization_Core),
                        SC("Cybersecurity","CS514",  SpecializationCourseRole.Specialization_Core),
                        SC("Cybersecurity","CS521",  SpecializationCourseRole.Specialization_Core),
                        SC("Cybersecurity","CS302",  SpecializationCourseRole.Specialization_Core),
                        SC("Cybersecurity","CS505",  SpecializationCourseRole.Elective),
                        SC("Cybersecurity","CS513",  SpecializationCourseRole.Elective),
                        SC("Cybersecurity","CS510",  SpecializationCourseRole.Elective),

                        // ── Software Engineering ─────────────────────────────────────────
                        SC("Software Engineering","CS101",  SpecializationCourseRole.Core),
                        SC("Software Engineering","CS102",  SpecializationCourseRole.Core),
                        SC("Software Engineering","CS103",  SpecializationCourseRole.Core),
                        SC("Software Engineering","CS201",  SpecializationCourseRole.Core),
                        SC("Software Engineering","CS203",  SpecializationCourseRole.Core),
                        SC("Software Engineering","CS204",  SpecializationCourseRole.Core),
                        SC("Software Engineering","CS205",  SpecializationCourseRole.Core),
                        SC("Software Engineering","CS301",  SpecializationCourseRole.Specialization_Core),
                        SC("Software Engineering","CS307",  SpecializationCourseRole.Specialization_Core),
                        SC("Software Engineering","CS405",  SpecializationCourseRole.Specialization_Core),
                        SC("Software Engineering","CS507",  SpecializationCourseRole.Specialization_Core),
                        SC("Software Engineering","CS523",  SpecializationCourseRole.Specialization_Core),
                        SC("Software Engineering","CS408",  SpecializationCourseRole.Specialization_Core),
                        SC("Software Engineering","CS409",  SpecializationCourseRole.Specialization_Core),
                        SC("Software Engineering","CS513",  SpecializationCourseRole.Elective),
                        SC("Software Engineering","CS512",  SpecializationCourseRole.Elective),
                        SC("Software Engineering","CS524",  SpecializationCourseRole.Elective),

                        // ── International Business (BE) ──────────────────────────────────
                        SC("International Business","BE101",SpecializationCourseRole.Core),
                        SC("International Business","BE102",SpecializationCourseRole.Core),
                        SC("International Business","BE103",SpecializationCourseRole.Core),
                        SC("International Business","BE104",SpecializationCourseRole.Core),
                        SC("International Business","BE106",SpecializationCourseRole.Core),
                        SC("International Business","BE203",SpecializationCourseRole.Core),
                        SC("International Business","BE205",SpecializationCourseRole.Core),
                        SC("International Business","BE303",SpecializationCourseRole.Specialization_Core),
                        SC("International Business","BE307",SpecializationCourseRole.Specialization_Core),
                        SC("International Business","BE302",SpecializationCourseRole.Specialization_Core),
                        SC("International Business","BE504",SpecializationCourseRole.Specialization_Core),
                        SC("International Business","BE505",SpecializationCourseRole.Specialization_Core),
                        SC("International Business","BE402",SpecializationCourseRole.Elective),
                        SC("International Business","BE501",SpecializationCourseRole.Elective),
                        SC("International Business","BE503",SpecializationCourseRole.Elective),

                        // ── Marketing (BE) ────────────────────────────────────────────────
                        SC("Marketing","BE101",SpecializationCourseRole.Core),
                        SC("Marketing","BE102",SpecializationCourseRole.Core),
                        SC("Marketing","BE103",SpecializationCourseRole.Core),
                        SC("Marketing","BE104",SpecializationCourseRole.Core),
                        SC("Marketing","BE205",SpecializationCourseRole.Core),
                        SC("Marketing","BE207",SpecializationCourseRole.Core),
                        SC("Marketing","BE404",SpecializationCourseRole.Specialization_Core),
                        SC("Marketing","BE302",SpecializationCourseRole.Specialization_Core),
                        SC("Marketing","BE306",SpecializationCourseRole.Specialization_Core),
                        SC("Marketing","BE501",SpecializationCourseRole.Specialization_Core),
                        SC("Marketing","BE402",SpecializationCourseRole.Elective),
                        SC("Marketing","BE504",SpecializationCourseRole.Elective),

                        // ── Finance (BE) ──────────────────────────────────────────────────
                        SC("Finance","BE101",SpecializationCourseRole.Core),
                        SC("Finance","BE102",SpecializationCourseRole.Core),
                        SC("Finance","BE104",SpecializationCourseRole.Core),
                        SC("Finance","BE106",SpecializationCourseRole.Core),
                        SC("Finance","BE203",SpecializationCourseRole.Core),
                        SC("Finance","BE207",SpecializationCourseRole.Core),
                        SC("Finance","BE301",SpecializationCourseRole.Specialization_Core),
                        SC("Finance","BE403",SpecializationCourseRole.Specialization_Core),
                        SC("Finance","BE502",SpecializationCourseRole.Specialization_Core),
                        SC("Finance","BE302",SpecializationCourseRole.Specialization_Core),
                        SC("Finance","BE402",SpecializationCourseRole.Elective),
                        SC("Finance","BE503",SpecializationCourseRole.Elective),

                        // ── Islamic Finance (BA) ──────────────────────────────────────────
                        SC("Islamic Finance","BA101",SpecializationCourseRole.Core),
                        SC("Islamic Finance","BA102",SpecializationCourseRole.Core),
                        SC("Islamic Finance","BA103",SpecializationCourseRole.Core),
                        SC("Islamic Finance","BA104",SpecializationCourseRole.Core),
                        SC("Islamic Finance","BA106",SpecializationCourseRole.Core),
                        SC("Islamic Finance","BA203",SpecializationCourseRole.Core),
                        SC("Islamic Finance","BA301",SpecializationCourseRole.Specialization_Core),
                        SC("Islamic Finance","BA303",SpecializationCourseRole.Specialization_Core),
                        SC("Islamic Finance","BA403",SpecializationCourseRole.Specialization_Core),
                        SC("Islamic Finance","BA502",SpecializationCourseRole.Specialization_Core),
                        SC("Islamic Finance","BA402",SpecializationCourseRole.Elective),
                        SC("Islamic Finance","BA504",SpecializationCourseRole.Elective),

                        // ── Marketing Arabic (BA) ─────────────────────────────────────────
                        SC("Marketing (Arabic)","BA101",SpecializationCourseRole.Core),
                        SC("Marketing (Arabic)","BA102",SpecializationCourseRole.Core),
                        SC("Marketing (Arabic)","BA103",SpecializationCourseRole.Core),
                        SC("Marketing (Arabic)","BA205",SpecializationCourseRole.Core),
                        SC("Marketing (Arabic)","BA207",SpecializationCourseRole.Core),
                        SC("Marketing (Arabic)","BA307",SpecializationCourseRole.Specialization_Core),
                        SC("Marketing (Arabic)","BA404",SpecializationCourseRole.Specialization_Core),
                        SC("Marketing (Arabic)","BA302",SpecializationCourseRole.Specialization_Core),
                        SC("Marketing (Arabic)","BA306",SpecializationCourseRole.Specialization_Core),
                        SC("Marketing (Arabic)","BA402",SpecializationCourseRole.Elective),
                        SC("Marketing (Arabic)","BA504",SpecializationCourseRole.Elective),

                        // ── Management Arabic (BA) ────────────────────────────────────────
                        SC("Management (Arabic)","BA101",SpecializationCourseRole.Core),
                        SC("Management (Arabic)","BA103",SpecializationCourseRole.Core),
                        SC("Management (Arabic)","BA104",SpecializationCourseRole.Core),
                        SC("Management (Arabic)","BA206",SpecializationCourseRole.Core),
                        SC("Management (Arabic)","BA302",SpecializationCourseRole.Specialization_Core),
                        SC("Management (Arabic)","BA304",SpecializationCourseRole.Specialization_Core),
                        SC("Management (Arabic)","BA305",SpecializationCourseRole.Specialization_Core),
                        SC("Management (Arabic)","BA401",SpecializationCourseRole.Specialization_Core),
                        SC("Management (Arabic)","BA402",SpecializationCourseRole.Elective),
                        SC("Management (Arabic)","BA503",SpecializationCourseRole.Elective),

                        // ── Digital and Multimedia Journalism ─────────────────────────────
                        SC("Digital and Multimedia Journalism","JR101",SpecializationCourseRole.Core),
                        SC("Digital and Multimedia Journalism","JR102",SpecializationCourseRole.Core),
                        SC("Digital and Multimedia Journalism","JR103",SpecializationCourseRole.Core),
                        SC("Digital and Multimedia Journalism","JR104",SpecializationCourseRole.Core),
                        SC("Digital and Multimedia Journalism","JR108",SpecializationCourseRole.Core),
                        SC("Digital and Multimedia Journalism","JR201",SpecializationCourseRole.Core),
                        SC("Digital and Multimedia Journalism","JR204",SpecializationCourseRole.Core),
                        SC("Digital and Multimedia Journalism","JR302",SpecializationCourseRole.Specialization_Core),
                        SC("Digital and Multimedia Journalism","JR303",SpecializationCourseRole.Specialization_Core),
                        SC("Digital and Multimedia Journalism","JR304",SpecializationCourseRole.Specialization_Core),
                        SC("Digital and Multimedia Journalism","JR404",SpecializationCourseRole.Specialization_Core),
                        SC("Digital and Multimedia Journalism","JR505",SpecializationCourseRole.Specialization_Core),
                        SC("Digital and Multimedia Journalism","JR501",SpecializationCourseRole.Elective),
                        SC("Digital and Multimedia Journalism","JR502",SpecializationCourseRole.Elective),
                        SC("Digital and Multimedia Journalism","JR504",SpecializationCourseRole.Elective),

                        // ── Broadcast Journalism ──────────────────────────────────────────
                        SC("Broadcast Journalism","JR101",SpecializationCourseRole.Core),
                        SC("Broadcast Journalism","JR102",SpecializationCourseRole.Core),
                        SC("Broadcast Journalism","JR103",SpecializationCourseRole.Core),
                        SC("Broadcast Journalism","JR108",SpecializationCourseRole.Core),
                        SC("Broadcast Journalism","JR202",SpecializationCourseRole.Core),
                        SC("Broadcast Journalism","JR208",SpecializationCourseRole.Specialization_Core),
                        SC("Broadcast Journalism","JR307",SpecializationCourseRole.Specialization_Core),
                        SC("Broadcast Journalism","JR308",SpecializationCourseRole.Specialization_Core),
                        SC("Broadcast Journalism","JR302",SpecializationCourseRole.Specialization_Core),
                        SC("Broadcast Journalism","JR401",SpecializationCourseRole.Specialization_Core),
                        SC("Broadcast Journalism","JR501",SpecializationCourseRole.Elective),
                        SC("Broadcast Journalism","JR503",SpecializationCourseRole.Elective),

                        // ── Public Relations and Advertising ─────────────────────────────
                        SC("Public Relations and Advertising","JR101",SpecializationCourseRole.Core),
                        SC("Public Relations and Advertising","JR102",SpecializationCourseRole.Core),
                        SC("Public Relations and Advertising","JR103",SpecializationCourseRole.Core),
                        SC("Public Relations and Advertising","JR108",SpecializationCourseRole.Core),
                        SC("Public Relations and Advertising","JR205",SpecializationCourseRole.Specialization_Core),
                        SC("Public Relations and Advertising","JR206",SpecializationCourseRole.Specialization_Core),
                        SC("Public Relations and Advertising","JR207",SpecializationCourseRole.Specialization_Core),
                        SC("Public Relations and Advertising","JR305",SpecializationCourseRole.Specialization_Core),
                        SC("Public Relations and Advertising","JR402",SpecializationCourseRole.Specialization_Core),
                        SC("Public Relations and Advertising","JR403",SpecializationCourseRole.Elective),
                        SC("Public Relations and Advertising","JR504",SpecializationCourseRole.Elective),

                        // ── Civil Engineering ─────────────────────────────────────────────
                        // Core = preparatory + common years
                        SC("Civil Engineering","ENG001",SpecializationCourseRole.Core),
                        SC("Civil Engineering","ENG002",SpecializationCourseRole.Core),
                        SC("Civil Engineering","ENG003",SpecializationCourseRole.Core),
                        SC("Civil Engineering","ENG004",SpecializationCourseRole.Core),
                        SC("Civil Engineering","ENG005",SpecializationCourseRole.Core),
                        SC("Civil Engineering","ENG006",SpecializationCourseRole.Core),
                        SC("Civil Engineering","ENG007",SpecializationCourseRole.Core),
                        SC("Civil Engineering","ENG008",SpecializationCourseRole.Core),
                        SC("Civil Engineering","ENG101",SpecializationCourseRole.Core),
                        SC("Civil Engineering","ENG102",SpecializationCourseRole.Core),
                        SC("Civil Engineering","ENG104",SpecializationCourseRole.Core),
                        SC("Civil Engineering","ENG201",SpecializationCourseRole.Core),
                        SC("Civil Engineering","ENG202",SpecializationCourseRole.Core),
                        SC("Civil Engineering","ENG203",SpecializationCourseRole.Core),
                        // Specialization Core = Civil-specific Years 3-4
                        SC("Civil Engineering","ENG301",SpecializationCourseRole.Specialization_Core),
                        SC("Civil Engineering","ENG302",SpecializationCourseRole.Specialization_Core),
                        SC("Civil Engineering","ENG303",SpecializationCourseRole.Specialization_Core),
                        SC("Civil Engineering","ENG304",SpecializationCourseRole.Specialization_Core),
                        SC("Civil Engineering","ENG305",SpecializationCourseRole.Specialization_Core),
                        SC("Civil Engineering","ENG306",SpecializationCourseRole.Specialization_Core),
                        SC("Civil Engineering","ENG307",SpecializationCourseRole.Specialization_Core),
                        SC("Civil Engineering","ENG401",SpecializationCourseRole.Specialization_Core),
                        SC("Civil Engineering","ENG402",SpecializationCourseRole.Specialization_Core),
                        SC("Civil Engineering","ENG403",SpecializationCourseRole.Specialization_Core),
                        SC("Civil Engineering","ENG404",SpecializationCourseRole.Specialization_Core),
                        SC("Civil Engineering","ENG405",SpecializationCourseRole.Specialization_Core),
                        SC("Civil Engineering","ENG408",SpecializationCourseRole.Specialization_Core),
                        SC("Civil Engineering","ENG409",SpecializationCourseRole.Specialization_Core),
                        SC("Civil Engineering","ENG501",SpecializationCourseRole.Elective),
                        SC("Civil Engineering","ENG503",SpecializationCourseRole.Elective),
                        SC("Civil Engineering","ENG505",SpecializationCourseRole.Elective),

                        // ── Electrical Engineering ────────────────────────────────────────
                        SC("Electrical Engineering","ENG001",SpecializationCourseRole.Core),
                        SC("Electrical Engineering","ENG002",SpecializationCourseRole.Core),
                        SC("Electrical Engineering","ENG003",SpecializationCourseRole.Core),
                        SC("Electrical Engineering","ENG005",SpecializationCourseRole.Core),
                        SC("Electrical Engineering","ENG006",SpecializationCourseRole.Core),
                        SC("Electrical Engineering","ENG007",SpecializationCourseRole.Core),
                        SC("Electrical Engineering","ENG008",SpecializationCourseRole.Core),
                        SC("Electrical Engineering","ENG103",SpecializationCourseRole.Core),
                        SC("Electrical Engineering","ENG104",SpecializationCourseRole.Core),
                        SC("Electrical Engineering","ENG201",SpecializationCourseRole.Core),
                        SC("Electrical Engineering","ENG204",SpecializationCourseRole.Core),
                        SC("Electrical Engineering","ENG207",SpecializationCourseRole.Core),
                        SC("Electrical Engineering","ENG311",SpecializationCourseRole.Specialization_Core),
                        SC("Electrical Engineering","ENG312",SpecializationCourseRole.Specialization_Core),
                        SC("Electrical Engineering","ENG313",SpecializationCourseRole.Specialization_Core),
                        SC("Electrical Engineering","ENG314",SpecializationCourseRole.Specialization_Core),
                        SC("Electrical Engineering","ENG315",SpecializationCourseRole.Specialization_Core),
                        SC("Electrical Engineering","ENG316",SpecializationCourseRole.Specialization_Core),
                        SC("Electrical Engineering","ENG317",SpecializationCourseRole.Specialization_Core),
                        SC("Electrical Engineering","ENG411",SpecializationCourseRole.Specialization_Core),
                        SC("Electrical Engineering","ENG412",SpecializationCourseRole.Specialization_Core),
                        SC("Electrical Engineering","ENG413",SpecializationCourseRole.Specialization_Core),
                        SC("Electrical Engineering","ENG414",SpecializationCourseRole.Specialization_Core),
                        SC("Electrical Engineering","ENG415",SpecializationCourseRole.Specialization_Core),
                        SC("Electrical Engineering","ENG418",SpecializationCourseRole.Specialization_Core),
                        SC("Electrical Engineering","ENG419",SpecializationCourseRole.Specialization_Core),
                        SC("Electrical Engineering","ENG502",SpecializationCourseRole.Elective),
                        SC("Electrical Engineering","ENG504",SpecializationCourseRole.Elective),
                        SC("Electrical Engineering","ENG505",SpecializationCourseRole.Elective),
                    };

                    var validScs = scs.Where(s => s != null).Cast<SpecializationCourse>().ToList();
                    if (validScs.Any())
                    {
                        await _dbContext.SpecializationCourses.AddRangeAsync(validScs);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Seeder Error: " + ex.Message);
                throw;
            }
        }

        public async Task SeedIdentityDataAsync()
{
    var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();
    if (pendingMigrations.Any())
        await _dbContext.Database.MigrateAsync();

    string[] roleNames = { "Admin", "Student", "Instructor", "InstructorAssistant" };
    foreach (var roleName in roleNames)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
            await _roleManager.CreateAsync(new Role { Name = roleName });
    }

    if (!_userManager.Users.Any())
    {
        var user = new User()
        {
            Email       = "admin@akhbaracademy.com",
            UserName    = "Admin123",
            PhoneNumber = "0123456789",
        };

        // Step 1: create the User via Identity (hashes password, assigns Id, etc.)
        var result = await _userManager.CreateAsync(user, "Admin@123");

        if (result.Succeeded)
        {
            // Step 2: create the Admin profile and link it to the created User
            var adminProfile = new Admin()
            {
                UserId = user.Id  // user.Id is now populated after CreateAsync
            };

            await _dbContext.Admins.AddAsync(adminProfile);
            await _dbContext.SaveChangesAsync();

            // Step 3: assign the role to the User
            await _userManager.AddToRoleAsync(user, "Admin");
        }
    }
}
    }
}