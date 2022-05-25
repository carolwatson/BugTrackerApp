using BugTracker.Models;
using BugTracker.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Data
{
    public static class DataUtility
    {
        //Company Ids
        private static int company1Id;
        private static int company2Id;
        private static int company3Id;
        private static int company4Id;
        private static int company5Id;


        //addsmarterasp link
        //public static string GetConnectionString(IConfiguration configuration)
        //{
        //    //The default connection string will come from appSettings like usual
        //    var connectionString = configuration.GetConnectionString("DefaultConnection");
        //    //It will be automatically overwritten if we are running on Heroku
        //    var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
        //    return string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);
        //}

        //public static string BuildConnectionString(string databaseUrl)
        //{
        //    //Provides an object representation of a uniform resource identifier (URI) and easy access to the parts of the URI.
        //    var databaseUri = new Uri(databaseUrl);
        //    var userInfo = databaseUri.UserInfo.Split(':');
        //    //Provides a simple way to create and manage the contents of connection strings used by the NpgsqlConnection class.
        //    var builder = new NpgsqlConnectionStringBuilder
        //    {
        //        Host = databaseUri.Host,
        //        Port = databaseUri.Port,
        //        Username = userInfo[0],
        //        Password = userInfo[1],
        //        Database = databaseUri.LocalPath.TrimStart('/'),
        //        SslMode = SslMode.Prefer,
        //        TrustServerCertificate = true
        //    };
        //    return builder.ToString();
        //}

        public static async Task ManageDataAsync(IHost host)
        {
            using var svcScope = host.Services.CreateScope();
            var svcProvider = svcScope.ServiceProvider;
            //Service: An instance of RoleManager
            var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();
            //Service: An instance of RoleManager
            var roleManagerSvc = svcProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //Service: An instance of the UserManager
            var userManagerSvc = svcProvider.GetRequiredService<UserManager<BTUser>>();
            //Migration: This is the programmatic equivalent to Update-Database
            await dbContextSvc.Database.MigrateAsync();


            //Custom Bug Tracker Seed Methods
            await SeedRolesAsync(roleManagerSvc);
            await SeedDefaultCompaniesAsync(dbContextSvc);
            await SeedDefaultUsersAsync(userManagerSvc);
            await SeedDemoUsersAsync(userManagerSvc);
            await SeedDefaultTicketTypeAsync(dbContextSvc);
            await SeedDefaultTicketStatusAsync(dbContextSvc);
            await SeedDefaultTicketPriorityAsync(dbContextSvc);
            await SeedDefaultProjectPriorityAsync(dbContextSvc);
            await SeedDefautProjectsAsync(dbContextSvc);
            await SeedDefautTicketsAsync(dbContextSvc);
        }


        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(nameof(Roles.Admin)));
            await roleManager.CreateAsync(new IdentityRole(nameof(Roles.ProjectManager)));
            await roleManager.CreateAsync(new IdentityRole(nameof(Roles.Developer)));
            await roleManager.CreateAsync(new IdentityRole(nameof(Roles.Submitter)));
            await roleManager.CreateAsync(new IdentityRole(nameof(Roles.DemoUser)));
        }

        public static async Task SeedDefaultCompaniesAsync(ApplicationDbContext context)
        {
            try
            {
                IList<Company> defaultcompanies = new List<Company>() {
    new Company() { Name = "Company1", Description="This is default Company 1" },
    new Company() { Name = "Company2", Description="This is default Company 2" },
    new Company() { Name = "Company3", Description="This is default Company 3" },
    new Company() { Name = "Company4", Description="This is default Company 4" },
    new Company() { Name = "Company5", Description="This is default Company 5" }
    };

                var dbCompanies = context.Companies.Select(c => c.Name).ToList();
                await context.Companies.AddRangeAsync(defaultcompanies.Where(c => !dbCompanies.Contains(c.Name)));
                await context.SaveChangesAsync();

                //Get company Ids
                company1Id = context.Companies.FirstOrDefault(p => p.Name == "Company1").Id;
                company2Id = context.Companies.FirstOrDefault(p => p.Name == "Company2").Id;
                company3Id = context.Companies.FirstOrDefault(p => p.Name == "Company3").Id;
                company4Id = context.Companies.FirstOrDefault(p => p.Name == "Company4").Id;
                company5Id = context.Companies.FirstOrDefault(p => p.Name == "Company5").Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Companies.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }
        }

        public static async Task SeedDefaultProjectPriorityAsync(ApplicationDbContext context)
        {
            try
            {
                IList<Models.ProjectPriority> projectPriorities = new List<ProjectPriority>() {
    new ProjectPriority() { Name = BTProjectPriority.Low.ToString() },
    new ProjectPriority() { Name = BTProjectPriority.Medium.ToString() },
    new ProjectPriority() { Name = BTProjectPriority.High.ToString() },
    new ProjectPriority() { Name = BTProjectPriority.Urgent.ToString() },
    };

                var dbProjectPriorities = context.ProjectPriorities.Select(c => c.Name).ToList();
                await context.ProjectPriorities.AddRangeAsync(projectPriorities.Where(c => !dbProjectPriorities.Contains(c.Name)));
                await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Project Priorities.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }
        }

        public static async Task SeedDefautProjectsAsync(ApplicationDbContext context)
        {

            //Get project priority Ids
            int priorityLow = context.ProjectPriorities.FirstOrDefault(p => p.Name == BTProjectPriority.Low.ToString()).Id;
            int priorityMedium = context.ProjectPriorities.FirstOrDefault(p => p.Name == BTProjectPriority.Medium.ToString()).Id;
            int priorityHigh = context.ProjectPriorities.FirstOrDefault(p => p.Name == BTProjectPriority.High.ToString()).Id;
            int priorityUrgent = context.ProjectPriorities.FirstOrDefault(p => p.Name == BTProjectPriority.Urgent.ToString()).Id;

            try
            {
                IList<Project> projects = new List<Project>() {
    new Project()
    {
    CompanyId = company1Id,
    Name = "Build a Personal Porfolio",
    Description="Single page html, css & javascript page. Serves as a landing page for candidates and contains a bio and links to all applications and challenges." ,
    StartDate = new DateTime(2021,8,20),
    EndDate = new DateTime(2021,8,20).AddMonths(1),
    ProjectPriorityId = priorityLow
    },
    new Project()
    {
    CompanyId = company2Id,
    Name = "Build a supplemental Blog Web Application",
    Description="Candidate's custom built web application using .Net Core with MVC, a postgres database and hosted in a heroku container. The app is designed for the candidate to create, update and maintain a live blog site.",
    StartDate = new DateTime(2021,8,20),
    EndDate = new DateTime(2021,8,20).AddMonths(4),
    ProjectPriorityId = priorityMedium
    },
    new Project()
    {
    CompanyId = company1Id,
    Name = "Build an Issue Tracking Web Application",
    Description="A custom designed .Net Core application with postgres database. The application is a multi tennent application designed to track issue tickets' progress. Implemented with identity and user roles, Tickets are maintained in projects which are maintained by users in the role of projectmanager. Each project has a team and team members.",
    StartDate = new DateTime(2021,8,20),
    EndDate = new DateTime(2021,8,20).AddMonths(6),
    ProjectPriorityId = priorityHigh
    },
    new Project()
    {
    CompanyId = company2Id,
    Name = "Build an Address Book Web Application",
    Description="A custom designed .Net Core application with postgres database. This is an application to serve as a rolodex of contacts for a given user..",
    StartDate = new DateTime(2021,8,20),
    EndDate = new DateTime(2021,8,20).AddMonths(2),
    ProjectPriorityId = priorityLow
    },
    new Project()
    {
    CompanyId = company1Id,
    Name = "Build a Movie Information Web Application",
    Description="A custom designed .Net Core application with postgres database. An API based application allows users to input and import movie posters and details including cast and crew information.",
    StartDate = new DateTime(2021,8,20),
    EndDate = new DateTime(2021,8,20).AddMonths(3),
    ProjectPriorityId = priorityHigh
    },
    new Project()
    {
    CompanyId = company2Id,
    Name = "Build a Password Generator",
    Description="A custom designed .Net Core application with postgres database. This is an application to serve as a rolodex of contacts for a given user..",
    StartDate = new DateTime(2021,8,20),
    EndDate = new DateTime(2021,8,20).AddMonths(2),
    ProjectPriorityId = priorityLow
    },
    new Project()
    {
    CompanyId = company2Id,
    Name = "Build a Mortgage Calculator",
    Description="A custom designed .Net Core application with postgres database. This is an application to serve as a rolodex of contacts for a given user..",
    StartDate = new DateTime(2021,8,20),
    EndDate = new DateTime(2021,8,20).AddMonths(2),
    ProjectPriorityId = priorityLow
    }
    };

                var dbProjects = context.Projects.Select(c => c.Name).ToList();
                await context.Projects.AddRangeAsync(projects.Where(c => !dbProjects.Contains(c.Name)));
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Projects.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }
        }

        public static async Task SeedDefaultUsersAsync(UserManager<BTUser> userManager)
        {
            //Seed Default Admin User
            var defaultUser = new BTUser
            {
                UserName = "btadmin1@cpwbugtracker.com",
                Email = "btadmin1@cpwbugtracker.com",
                FirstName = "Bill",
                LastName = "Appuser",
                EmailConfirmed = true,
                CompanyId = company1Id
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Default Admin User.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }

            //Seed Default Admin User
            defaultUser = new BTUser
            {
                UserName = "btadmin2@cpwbugtracker.com",
                Email = "btadmin2@cpwbugtracker.com",
                FirstName = "Steve",
                LastName = "Appuser",
                EmailConfirmed = true,
                CompanyId = company2Id
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Default Admin User.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }


            //Seed Default ProjectManager1 User
            defaultUser = new BTUser
            {
                UserName = "ProjectManager1@cpwbugtracker.com",
                Email = "ProjectManager1@cpwbugtracker.com",
                FirstName = "John",
                LastName = "Appuser",
                EmailConfirmed = true,
                CompanyId = company1Id
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.ProjectManager.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Default ProjectManager1 User.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }


            //Seed Default ProjectManager2 User
            defaultUser = new BTUser
            {
                UserName = "ProjectManager2@cpwbugtracker.com",
                Email = "ProjectManager2@cpwbugtracker.com",
                FirstName = "Jane",
                LastName = "Appuser",
                EmailConfirmed = true,
                CompanyId = company2Id
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.ProjectManager.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Default ProjectManager2 User.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }


            //Seed Default Developer1 User
            defaultUser = new BTUser
            {
                UserName = "Developer1@cpwbugtracker.com",
                Email = "Developer1@cpwbugtracker.com",
                FirstName = "Elon",
                LastName = "Appuser",
                EmailConfirmed = true,
                CompanyId = company1Id
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Default Developer1 User.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }


            //Seed Default Developer2 User
            defaultUser = new BTUser
            {
                UserName = "Developer2@cpwbugtracker.com",
                Email = "Developer2@cpwbugtracker.com",
                FirstName = "James",
                LastName = "Appuser",
                EmailConfirmed = true,
                CompanyId = company2Id
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Default Developer2 User.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }


            //Seed Default Developer3 User
            defaultUser = new BTUser
            {
                UserName = "Developer3@cpwbugtracker.com",
                Email = "Developer3@cpwbugtracker.com",
                FirstName = "Natasha",
                LastName = "Appuser",
                EmailConfirmed = true,
                CompanyId = company1Id
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Default Developer3 User.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }


            //Seed Default Developer4 User
            defaultUser = new BTUser
            {
                UserName = "Developer4@cpwbugtracker.com",
                Email = "Developer4@cpwbugtracker.com",
                FirstName = "Carol",
                LastName = "Appuser",
                EmailConfirmed = true,
                CompanyId = company2Id
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Default Developer4 User.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }


            //Seed Default Developer5 User
            defaultUser = new BTUser
            {
                UserName = "Developer5@cpwbugtracker.com",
                Email = "Developer5@cpwbugtracker.com",
                FirstName = "Tony",
                LastName = "Appuser",
                EmailConfirmed = true,
                CompanyId = company1Id
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Default Developer5 User.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }

            //Seed Default Developer6 User
            defaultUser = new BTUser
            {
                UserName = "Developer6@cpwbugtracker.com",
                Email = "Developer6@cpwbugtracker.com",
                FirstName = "Bruce",
                LastName = "Appuser",
                EmailConfirmed = true,
                CompanyId = company2Id
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Default Developer5 User.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }

            //Seed Default Submitter1 User
            defaultUser = new BTUser
            {
                UserName = "Submitter1@cpwbugtracker.com",
                Email = "Submitter1@cpwbugtracker.com",
                FirstName = "Scott",
                LastName = "Appuser",
                EmailConfirmed = true,
                CompanyId = company1Id
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Submitter.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Default Submitter1 User.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }


            //Seed Default Submitter2 User
            defaultUser = new BTUser
            {
                UserName = "Submitter2@cpwbugtracker.com",
                Email = "Submitter2@cpwbugtracker.com",
                FirstName = "Sue",
                LastName = "Appuser",
                EmailConfirmed = true,
                CompanyId = company2Id
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Submitter.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Default Submitter2 User.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }

        }

        public static async Task SeedDemoUsersAsync(UserManager<BTUser> userManager)
        {
            //Seed Demo Admin User
            var defaultUser = new BTUser
            {
                UserName = "demoadmin@cpwbugtracker.com",
                Email = "demoadmin@cpwbugtracker.com",
                FirstName = "Demo",
                LastName = "Admin",
                EmailConfirmed = true,
                CompanyId = company1Id
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.DemoUser.ToString());

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Demo Admin User.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }


            //Seed Demo ProjectManager User
            defaultUser = new BTUser
            {
                UserName = "demopm@cpwbugtracker.com",
                Email = "demopm@cpwbugtracker.com",
                FirstName = "Demo",
                LastName = "ProjectManager",
                EmailConfirmed = true,
                CompanyId = company2Id
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.ProjectManager.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.DemoUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Demo ProjectManager1 User.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }


            //Seed Demo Developer User
            defaultUser = new BTUser
            {
                UserName = "demodev@cpwbugtracker.com",
                Email = "demodev@cpwbugtracker.com",
                FirstName = "Demo",
                LastName = "Developer",
                EmailConfirmed = true,
                CompanyId = company2Id
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Developer.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.DemoUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Demo Developer1 User.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }


            //Seed Demo Submitter User
            defaultUser = new BTUser
            {
                UserName = "demosub@cpwbugtracker.com",
                Email = "demosub@cpwbugtracker.com",
                FirstName = "Demo",
                LastName = "Submitter",
                EmailConfirmed = true,
                CompanyId = company2Id
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Submitter.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.DemoUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Demo Submitter User.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }


            //Seed Demo New User
            defaultUser = new BTUser
            {
                UserName = "demonew@cpwbugtracker.com",
                Email = "demonew@cpwbugtracker.com",
                FirstName = "Demo",
                LastName = "NewUser",
                EmailConfirmed = true,
                CompanyId = company2Id
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Abc&123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Submitter.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.DemoUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Demo New User.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }
        }


        public static async Task SeedDefaultTicketTypeAsync(ApplicationDbContext context)
        {
            try
            {
                IList<TicketType> ticketTypes = new List<TicketType>() {
    new TicketType() { Name = BTTicketType.NewDevelopment.ToString() }, // Ticket involves development of a new, uncoded solution 
	new TicketType() { Name = BTTicketType.WorkTask.ToString() }, // Ticket involves development of the specific ticket description 
	new TicketType() { Name = BTTicketType.Defect.ToString()}, // Ticket involves unexpected development/maintenance on a previously designed feature/functionality
	new TicketType() { Name = BTTicketType.ChangeRequest.ToString() }, // Ticket involves modification development of a previously designed feature/functionality
	new TicketType() { Name = BTTicketType.Enhancement.ToString() }, // Ticket involves additional development on a previously designed feature or new functionality
	new TicketType() { Name = BTTicketType.GeneralTask.ToString() } // Ticket involves no software development but may involve tasks such as configuations, or hardware setup
	};

                var dbTicketTypes = context.TicketTypes.Select(c => c.Name).ToList();
                await context.TicketTypes.AddRangeAsync(ticketTypes.Where(c => !dbTicketTypes.Contains(c.Name)));
                await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Ticket Types.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }
        }

        public static async Task SeedDefaultTicketStatusAsync(ApplicationDbContext context)
        {
            try
            {
                IList<TicketStatus> ticketStatuses = new List<TicketStatus>() {
    new TicketStatus() { Name = BTTicketStatus.New.ToString() }, // Newly Created ticket having never been assigned
	new TicketStatus() { Name = BTTicketStatus.Development.ToString() }, // Ticket is assigned and currently being worked 
	new TicketStatus() { Name = BTTicketStatus.Testing.ToString() }, // Ticket is assigned and is currently being tested
	new TicketStatus() { Name = BTTicketStatus.Resolved.ToString() }, // Ticket remains assigned to the developer but work in now complete
	};

                var dbTicketStatuses = context.TicketStatuses.Select(c => c.Name).ToList();
                await context.TicketStatuses.AddRangeAsync(ticketStatuses.Where(c => !dbTicketStatuses.Contains(c.Name)));
                await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Ticket Statuses.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }
        }

        public static async Task SeedDefaultTicketPriorityAsync(ApplicationDbContext context)
        {
            try
            {
                IList<TicketPriority> ticketPriorities = new List<TicketPriority>() {
    new TicketPriority() { Name = BTTicketPriority.Low.ToString() },
    new TicketPriority() { Name = BTTicketPriority.Medium.ToString() },
    new TicketPriority() { Name = BTTicketPriority.High.ToString()},
    new TicketPriority() { Name = BTTicketPriority.Urgent.ToString()},
    };

                var dbTicketPriorities = context.TicketPriorities.Select(c => c.Name).ToList();
                await context.TicketPriorities.AddRangeAsync(ticketPriorities.Where(c => !dbTicketPriorities.Contains(c.Name)));
                context.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Ticket Priorities.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }
        }


        public static async Task SeedDefautTicketsAsync(ApplicationDbContext context)
        {
            //Get project Ids
            int portfolioId = context.Projects.FirstOrDefault(p => p.Name == "Build a Personal Porfolio").Id;
            int blogId = context.Projects.FirstOrDefault(p => p.Name == "Build a supplemental Blog Web Application").Id;
            int bugtrackerId = context.Projects.FirstOrDefault(p => p.Name == "Build an Issue Tracking Web Application").Id;
            int movieId = context.Projects.FirstOrDefault(p => p.Name == "Build a Movie Information Web Application").Id;
            int passwordStoreId = context.Projects.FirstOrDefault(p => p.Name == "Build a Password Generator").Id;
            int mortgageCalculatorId = context.Projects.FirstOrDefault(p => p.Name == "Build a Mortgage Calculator").Id;

            //Get ticket type Ids
            int NewDevelopment = context.TicketTypes.FirstOrDefault(p => p.Name == BTTicketType.NewDevelopment.ToString()).Id;
            int WorkTask = context.TicketTypes.FirstOrDefault(p => p.Name == BTTicketType.WorkTask.ToString()).Id;
            int Defect = context.TicketTypes.FirstOrDefault(p => p.Name == BTTicketType.Defect.ToString()).Id;
            int Enhancement = context.TicketTypes.FirstOrDefault(p => p.Name == BTTicketType.Enhancement.ToString()).Id;
            int ChangeRequest = context.TicketTypes.FirstOrDefault(p => p.Name == BTTicketType.ChangeRequest.ToString()).Id;
            int GeneralTask = context.TicketTypes.FirstOrDefault(p => p.Name == BTTicketType.GeneralTask.ToString()).Id;

            //Get ticket priority Ids
            int Low = context.TicketPriorities.FirstOrDefault(p => p.Name == BTTicketPriority.Low.ToString()).Id;
            int Medium = context.TicketPriorities.FirstOrDefault(p => p.Name == BTTicketPriority.Medium.ToString()).Id;
            int High = context.TicketPriorities.FirstOrDefault(p => p.Name == BTTicketPriority.High.ToString()).Id;
            int Urgent = context.TicketPriorities.FirstOrDefault(p => p.Name == BTTicketPriority.Urgent.ToString()).Id;

            //Get ticket status Ids
            int New = context.TicketStatuses.FirstOrDefault(p => p.Name == BTTicketStatus.New.ToString()).Id;
            int Development = context.TicketStatuses.FirstOrDefault(p => p.Name == BTTicketStatus.Development.ToString()).Id;
            int Testing = context.TicketStatuses.FirstOrDefault(p => p.Name == BTTicketStatus.Testing.ToString()).Id;
            int Resolved = context.TicketStatuses.FirstOrDefault(p => p.Name == BTTicketStatus.Resolved.ToString()).Id;


            try
            {
                IList<Ticket> tickets = new List<Ticket>() {
	
	//PORTFOLIO
	new Ticket() {Name = "Find Template", Description = "Find Template", Created = DateTimeOffset.Now, ProjectId = portfolioId, TicketPriorityId = Low, TicketStatusId = Resolved, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "Modify CSS", Description = "Modify CSS", Created = DateTimeOffset.Now, ProjectId = portfolioId, TicketPriorityId = Medium, TicketStatusId = Resolved, TicketTypeId = ChangeRequest},
    new Ticket() {Name = "Create About Section", Description = "Create About Section", Created = DateTimeOffset.Now, ProjectId = portfolioId, TicketPriorityId = Medium, TicketStatusId = Resolved, TicketTypeId = Enhancement},
    new Ticket() {Name = "Create Resume Section", Description = "Create Resume Section", Created = DateTimeOffset.Now, ProjectId = portfolioId, TicketPriorityId = Medium, TicketStatusId = Resolved, TicketTypeId = Defect},
    new Ticket() {Name = "Add completed link to project", Description = "Add completed project links to project", Created = DateTimeOffset.Now, ProjectId = portfolioId, TicketPriorityId = Medium, TicketStatusId = Resolved, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "Add to smarterAsp", Description = "Add to smarterAsp", Created = DateTimeOffset.Now, ProjectId = portfolioId, TicketPriorityId = Medium, TicketStatusId = Development, TicketTypeId = NewDevelopment},
    //BLOG
	new Ticket() {Name = "Blog Ticket 1", Description = "Ticket details for blog ticket 1", Created = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = Low, TicketStatusId = New, TicketTypeId = Defect},
    new Ticket() {Name = "Blog Ticket 2", Description = "Ticket details for blog ticket 2", Created = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = Medium, TicketStatusId = Development, TicketTypeId = Enhancement},
    new Ticket() {Name = "Blog Ticket 3", Description = "Ticket details for blog ticket 3", Created = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = High, TicketStatusId = New, TicketTypeId = ChangeRequest},
    new Ticket() {Name = "Blog Ticket 4", Description = "Ticket details for blog ticket 4", Created = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = Urgent, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "Blog Ticket 5", Description = "Ticket details for blog ticket 5", Created = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = Low, TicketStatusId = Development, TicketTypeId = Defect},
    new Ticket() {Name = "Blog Ticket 6", Description = "Ticket details for blog ticket 6", Created = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = Enhancement},
    new Ticket() {Name = "Blog Ticket 7", Description = "Ticket details for blog ticket 7", Created = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = High, TicketStatusId = New, TicketTypeId = ChangeRequest},
    new Ticket() {Name = "Blog Ticket 8", Description = "Ticket details for blog ticket 8", Created = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = Urgent, TicketStatusId = Development, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "Blog Ticket 9", Description = "Ticket details for blog ticket 9", Created = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = Low, TicketStatusId = New, TicketTypeId = Defect},
    new Ticket() {Name = "Blog Ticket 10", Description = "Ticket details for blog ticket 10", Created = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = Enhancement},
    new Ticket() {Name = "Blog Ticket 11", Description = "Ticket details for blog ticket 11", Created = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = High, TicketStatusId = Development, TicketTypeId = ChangeRequest},
    new Ticket() {Name = "Blog Ticket 12", Description = "Ticket details for blog ticket 12", Created = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = Urgent, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "Blog Ticket 13", Description = "Ticket details for blog ticket 13", Created = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = Low, TicketStatusId = New, TicketTypeId = Defect},
    new Ticket() {Name = "Blog Ticket 14", Description = "Ticket details for blog ticket 14", Created = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = Medium, TicketStatusId = Development, TicketTypeId = Enhancement},
    new Ticket() {Name = "Blog Ticket 15", Description = "Ticket details for blog ticket 15", Created = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = High, TicketStatusId = New, TicketTypeId = ChangeRequest},
    new Ticket() {Name = "Blog Ticket 16", Description = "Ticket details for blog ticket 16", Created = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = Urgent, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "Blog Ticket 17", Description = "Ticket details for blog ticket 17", Created = DateTimeOffset.Now, ProjectId = blogId, TicketPriorityId = High, TicketStatusId = Development, TicketTypeId = NewDevelopment},
	//BUGTRACKER 
	new Ticket() {Name = "Remove Comapny CRUD from view", Description = "hide company model CRUD", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "fix login Modal", Description = "remove extra step in login", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
     new Ticket() {Name = "Remove Company CRUD from view", Description = "hide company model CRUD", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "fix login Modal", Description = "remove extra, step in login", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "rename emails, users", Description = "change email names in seeding", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "add super admin role", Description = "add super admin", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "Split External WEb connectionString", Description = "create data utility function for external website", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "move data to new library", Description = "move data (models, services) to new library", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "Create test Library", Description = "create test Library", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "fix demouser auto login", Description = "create functionality for all demo users", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "create invite service", Description = "create invite service", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "modify notification view", Description = "modify notification view", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "fix link on avatar", Description = "make avatar functional on dashboard", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "make charts optional", Description = "make charts optional by click", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "fix broken link", Description = "fix broken image link on Create View", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "Badd link to archive icon", Description = "add link to archive icon , Details View", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "remove title , AssignMember View", Description = "remove title , AssignMembers View", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "Bmodify side menubar", Description = "change style of side menubar on Layout", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "add datatables feature", Description = "add datatables feature for all list pages , 10 viewable", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "modify all project dates", Description = "change start  end date", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "Rename BugTracker", Description = "rename bugtracker to issueTracker thruout", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "switch view on dashboard", Description = "switch view on dashboard of Tickets and projects", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "fix broken link ", Description = "fix broken link on project title , Dashboard", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "Create Navigation Test Checklist", Description = "create checklist to test navigation , attach doc", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "Modify the Create pages", Description = "Modify the Create pages , Ticket,project", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "set up email notifications", Description = "set up email notifications to users", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "change to smarterASP email", Description = "set up smartasp email, use that email", Created = DateTimeOffset.Now, ProjectId = bugtrackerId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = NewDevelopment},

	
    
    //MOVIE
	new Ticket() {Name = "Movie Ticket 1", Description = "Ticket details for movie ticket 1", Created = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = Low, TicketStatusId = New, TicketTypeId = Defect},
    new Ticket() {Name = "Movie Ticket 2", Description = "Ticket details for movie ticket 2", Created = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = Medium, TicketStatusId = Development, TicketTypeId = Enhancement},
    new Ticket() {Name = "Movie Ticket 3", Description = "Ticket details for movie ticket 3", Created = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = High, TicketStatusId = New, TicketTypeId = ChangeRequest},
    new Ticket() {Name = "Movie Ticket 4", Description = "Ticket details for movie ticket 4", Created = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = Urgent, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "Movie Ticket 5", Description = "Ticket details for movie ticket 5", Created = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = Low, TicketStatusId = Development, TicketTypeId = Defect},
    new Ticket() {Name = "Movie Ticket 6", Description = "Ticket details for movie ticket 6", Created = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = Enhancement},
    new Ticket() {Name = "Movie Ticket 7", Description = "Ticket details for movie ticket 7", Created = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = High, TicketStatusId = New, TicketTypeId = ChangeRequest},
    new Ticket() {Name = "Movie Ticket 8", Description = "Ticket details for movie ticket 8", Created = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = Urgent, TicketStatusId = Development, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "Movie Ticket 9", Description = "Ticket details for movie ticket 9", Created = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = Low, TicketStatusId = New, TicketTypeId = Defect},
    new Ticket() {Name = "Movie Ticket 10", Description = "Ticket details for movie ticket 10", Created = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = Medium, TicketStatusId = New, TicketTypeId = Enhancement},
    new Ticket() {Name = "Movie Ticket 11", Description = "Ticket details for movie ticket 11", Created = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = High, TicketStatusId = Development, TicketTypeId = ChangeRequest},
    new Ticket() {Name = "Movie Ticket 12", Description = "Ticket details for movie ticket 12", Created = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = Urgent, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "Movie Ticket 13", Description = "Ticket details for movie ticket 13", Created = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = Low, TicketStatusId = New, TicketTypeId = Defect},
    new Ticket() {Name = "Movie Ticket 14", Description = "Ticket details for movie ticket 14", Created = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = Medium, TicketStatusId = Development, TicketTypeId = Enhancement},
    new Ticket() {Name = "Movie Ticket 15", Description = "Ticket details for movie ticket 15", Created = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = High, TicketStatusId = New, TicketTypeId = ChangeRequest},
    new Ticket() {Name = "Movie Ticket 16", Description = "Ticket details for movie ticket 16", Created = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = Urgent, TicketStatusId = New, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "Movie Ticket 17", Description = "Ticket details for movie ticket 17", Created = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = High, TicketStatusId = Development, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "Movie Ticket 18", Description = "Ticket details for movie ticket 18", Created = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = Medium, TicketStatusId = Development, TicketTypeId = Enhancement},
    new Ticket() {Name = "Movie Ticket 19", Description = "Ticket details for movie ticket 19", Created = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = High, TicketStatusId = New, TicketTypeId = ChangeRequest},
    new Ticket() {Name = "Movie Ticket 20", Description = "Ticket details for movie ticket 20", Created = DateTimeOffset.Now, ProjectId = movieId, TicketPriorityId = Urgent, TicketStatusId = New, TicketTypeId = NewDevelopment},

    //pswd
	new Ticket() {Name = "Create MVC combine with password tool", Description = "Create MVC combine with password tool", Created = DateTimeOffset.Now, ProjectId = passwordStoreId, TicketPriorityId = Low, TicketStatusId = New, TicketTypeId = Defect},
    new Ticket() {Name = "Add Code Explanation Page", Description = "Add Code Explanation Page", Created = DateTimeOffset.Now, ProjectId = passwordStoreId, TicketPriorityId = Medium, TicketStatusId = Development, TicketTypeId = Enhancement},
    new Ticket() {Name = "add datatables feature for all list pages", Description = "add datatables feature for all list pages", Created = DateTimeOffset.Now, ProjectId = passwordStoreId, TicketPriorityId = Low, TicketStatusId = New, TicketTypeId = Defect},
    new Ticket() {Name = "add password gen page separately", Description = "add password gen page separately", Created = DateTimeOffset.Now, ProjectId = passwordStoreId, TicketPriorityId = Medium, TicketStatusId = Development, TicketTypeId = Enhancement},
    new Ticket() {Name = "modify connectionservice.cs for external website", Description = "modify connectionservice.cs for external website", Created = DateTimeOffset.Now, ProjectId = passwordStoreId, TicketPriorityId = Low, TicketStatusId = New, TicketTypeId = Defect},
    new Ticket() {Name = "switch to sql for external site", Description = "switch to sql for external site", Created = DateTimeOffset.Now, ProjectId = passwordStoreId, TicketPriorityId = Medium, TicketStatusId = Development, TicketTypeId = Enhancement},
    new Ticket() {Name = "create separate random choices", Description = "create separate random choices", Created = DateTimeOffset.Now, ProjectId = passwordStoreId, TicketPriorityId = Low, TicketStatusId = New, TicketTypeId = Defect},
    new Ticket() {Name = "create custom buttons", Description = "create custom buttons", Created = DateTimeOffset.Now, ProjectId = passwordStoreId, TicketPriorityId = Medium, TicketStatusId = Development, TicketTypeId = Enhancement},
    new Ticket() {Name = "create image service", Description = "create image service", Created = DateTimeOffset.Now, ProjectId = passwordStoreId, TicketPriorityId = Medium, TicketStatusId = Resolved, TicketTypeId = ChangeRequest },
     new Ticket() {Name = "Find suitable image", Description = "Find suitable image", Created = DateTimeOffset.Now, ProjectId = passwordStoreId, TicketPriorityId = Medium, TicketStatusId = Resolved, TicketTypeId = Enhancement},
      new Ticket() {Name = "Create Main View", Description = "Create Main View", Created = DateTimeOffset.Now, ProjectId = passwordStoreId, TicketPriorityId = Medium, TicketStatusId = Resolved, TicketTypeId = Enhancement},
       new Ticket() {Name = "Add to smarterAsp", Description = "Add to smarterAsp", Created = DateTimeOffset.Now, ProjectId = passwordStoreId, TicketPriorityId = Medium, TicketStatusId = Development, TicketTypeId = Enhancement},

    //martg
		new Ticket() {Name = "Find Template", Description = "Find Template", Created = DateTimeOffset.Now, ProjectId = mortgageCalculatorId, TicketPriorityId = Low, TicketStatusId = Resolved, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "Modify CSS", Description = "Modify CSS", Created = DateTimeOffset.Now, ProjectId = mortgageCalculatorId, TicketPriorityId = Medium, TicketStatusId = Resolved, TicketTypeId = ChangeRequest},
    new Ticket() {Name = "Create Calculate MonthlyPayment Function", Description = "Create Calculate MonthlyPayment Function", Created = DateTimeOffset.Now, ProjectId = mortgageCalculatorId, TicketPriorityId = Medium, TicketStatusId = Resolved, TicketTypeId = Enhancement},
    new Ticket() {Name = "Add Code Explanation Page", Description = "Add Code Explanation Page", Created = DateTimeOffset.Now, ProjectId = mortgageCalculatorId, TicketPriorityId = Medium, TicketStatusId = Resolved, TicketTypeId = Defect},
    new Ticket() {Name = "Find suitable image", Description = "Find suitable image", Created = DateTimeOffset.Now, ProjectId = mortgageCalculatorId, TicketPriorityId = Medium, TicketStatusId = Resolved, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "Create Main View", Description = "Create Main View", Created = DateTimeOffset.Now, ProjectId = mortgageCalculatorId, TicketPriorityId = Medium, TicketStatusId = Resolved, TicketTypeId = NewDevelopment},
    new Ticket() {Name = "Add to smarterAsp", Description = "Add to smarterAsp", Created = DateTimeOffset.Now, ProjectId = mortgageCalculatorId, TicketPriorityId = Medium, TicketStatusId = Development, TicketTypeId = NewDevelopment},

    };


                var dbTickets = context.Tickets.Select(c => c.Name).ToList();
                await context.Tickets.AddRangeAsync(tickets.Where(c => !dbTickets.Contains(c.Name)));
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("************* ERROR *************");
                Console.WriteLine("Error Seeding Tickets.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***********************************");
                throw;
            }
        }

    }

}
