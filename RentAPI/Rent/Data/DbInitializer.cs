using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rent.DTOs;
using Rent.Models;
using Rent.Repositories;

namespace Rent.Data
{
    public class DbInitializer
    {
        public static void Initialize(RentContext context)
        {

            


            context.Database.EnsureCreated();

            if (context.User.Any())
            {
                return;
            }

            #region Permission InitializingPermissions

            string[] PermissionNames = new[] {
                "Team",
                "User",
                "Customer",
                "Location",
                "CleaningTask",
                "QualityReport",
                "CompletedTask",
                "Login",
                "Permission",
                "RegularTask",
                "WindowTask",
                "FanCoilTask",
                "Chat",
            };

            Dictionary<string, bool[]> MasterPermissions = new Dictionary<string, bool[]>
            {
                //NAME                                      Create Read   Update Delete
                {"Team",                        new bool[] {true, true, true, true, true }},
                {"User",                        new bool[] {true, true, true, true, true }},
                {"Customer",                    new bool[] {true, true, true, true, true }},
                {"Location",                    new bool[] {true, true, true, true, true }},
                {"CleaningTask",                new bool[] {true, true, true, true, true }},
                {"QualityReport",               new bool[] {true, true, true, true, true }},
                {"CompletedTask",               new bool[] {true, true, true, true, true }},
                {"Login",                       new bool[] {true, true, true, true, true }},
                {"Permission",                  new bool[] {true, true, true, true, true }},
                {"RegularTask",                 new bool[] {true, true, true, true, true }},
                {"WindowTask",                  new bool[] {true, true, true, true, true }},
                {"FanCoilTask",                 new bool[] {true, true, true, true, true }},
                {"Chat",                        new bool[] {true, true, true, true, true }}
            };

            Dictionary<string, bool[]> HumanResourcePermissions = new Dictionary<string, bool[]>
            {
                //NAME                                      Create Read Update Delete
                {"Team",                        new bool[] {true, true, true, true, true }},
                {"User",                        new bool[] {true, true, true, true, true }},
                {"Customer",                    new bool[] {true, true, true, true, true }},
                {"Location",                    new bool[] {true, true, true, true, true }},
                {"CleaningTask",                new bool[] {true, true, true, true, true }},
                {"QualityReport",               new bool[] {true, true, true, true, true }},
                {"CompletedTask",               new bool[] {true, true, true, true, true }},
                {"Login",                       new bool[] {true, true, true, true, true }},
                {"Permission",                  new bool[] {true, true, true, true, true }},
                {"RegularTask",                 new bool[] {true, true, true, true, true }},
                {"WindowTask",                  new bool[] {true, true, true, true, true }},
                {"FanCoilTask",                 new bool[] {true, true, true, true, true }},
                {"Chat",                        new bool[] {true, true, true, true, true }}
            };

            Dictionary<string, bool[]> ServiceLeaderPermissions = new Dictionary<string, bool[]>
            {
                //NAME                                      Create Read   Update Delete
                {"Team",                        new bool[] {false, true, true, false, false }},
                {"User",                        new bool[] {false, true, true, false, false }},
                {"Customer",                    new bool[] {false, true, true, false, false }},
                {"Location",                    new bool[] {false, true, true, false, false }},
                {"CleaningTask",                new bool[] {true, true, true, true, false }},
                {"QualityReport",               new bool[] {true, true, true, true, false }},
                {"CompletedTask",               new bool[] {true, true, true, true, false }},
                {"Login",                       new bool[] {false, false, false, false, false }},
                {"Permission",                  new bool[] {false, false, false, false, false }},
                {"RegularTask",                 new bool[] {true, true, true, true, false }},
                {"WindowTask",                  new bool[] {true, true, true, true, false }},
                {"FanCoilTask",                 new bool[] {true, true, true, true, false }},
                {"Chat",                        new bool[] {true, true, true, false, false }}
            };

            Dictionary<string, bool[]> SalesPermissions = new Dictionary<string, bool[]>
            {
                //NAME                                      Create Read   Update Delete
                {"Team",                        new bool[] {false, false, false, false, false }},
                {"User",                        new bool[] {false, false, false, false, false }},
                {"Customer",                    new bool[] {true, true, true, false, false }},
                {"Location",                    new bool[] {true, true, true, false, false }},
                {"CleaningTask",                new bool[] {true, true, true, true, false }},
                {"QualityReport",               new bool[] {false, false, false, false, false }},
                {"CompletedTask",               new bool[] {false, false, false, false, false }},
                {"Login",                       new bool[] {false, false, false, false, false }},
                {"Permission",                  new bool[] {false, false, false, false, false }},
                {"RegularTask",                 new bool[] {true, true, true, false, false }},
                {"WindowTask",                  new bool[] {true, true, true, false, false }},
                {"FanCoilTask",                 new bool[] {true, true, true, false, false }},
                {"Chat",                        new bool[] {true, true, true, false, false }}
            };

            Dictionary<string, bool[]> RegularCleaningAssistantPermissions = new Dictionary<string, bool[]>
            {
                //NAME                                      Create Read   Update Delete
                {"Team",                        new bool[] {false, false, false, false, false }},
                {"User",                        new bool[] {false, false, false, false, false }},
                {"Customer",                    new bool[] {false, true, false, false, false }},
                {"Location",                    new bool[] {false, true, false, false, false }},
                {"CleaningTask",                new bool[] {false, true, false, false, false }},
                {"QualityReport",               new bool[] {false, false, false, false, false }},
                {"CompletedTask",               new bool[] {false, false, false, false, false }},
                {"Login",                       new bool[] {false, false, false, false, false }},
                {"Permission",                  new bool[] {false, false, false, false, false }},
                {"RegularTask",                 new bool[] {false, true, false, false, false }},
                {"WindowTask",                  new bool[] {false, false, false, false, false }},
                {"FanCoilTask",                 new bool[] {false, false, false, false, false }},
                {"Chat",                        new bool[] {false, true, true, false, false }}
            };
            
            Dictionary<string, bool[]> WindowCleaningAssistantPermissions = new Dictionary<string, bool[]>
            {
                //NAME                                      Create Read   Update Delete
                {"Team",                        new bool[] {false, false, false, false, false }},
                {"User",                        new bool[] {false, false, false, false, false }},
                {"Customer",                    new bool[] {false, true, false, false, false }},
                {"Location",                    new bool[] {false, true, false, false, false }},
                {"CleaningTask",                new bool[] {false, true, false, false, false }},
                {"QualityReport",               new bool[] {false, false, false, false, false }},
                {"CompletedTask",               new bool[] {true, true, false, false, false }},
                {"Login",                       new bool[] {false, false, false, false, false }},
                {"Permission",                  new bool[] {false, false, false, false, false }},
                {"RegularTask",                 new bool[] {false, false, false, false, false }},
                {"WindowTask",                  new bool[] {false, true, false, false, false }},
                {"FanCoilTask",                 new bool[] {false, false, false, false, false }},
                {"Chat",                        new bool[] {false, true, true, false, false }}
            };

            Dictionary<string, bool[]> FanCoilCleaningAssistantPermissions = new Dictionary<string, bool[]>
            {
                //NAME                                      Create Read   Update Delete
                {"Team",                        new bool[] {false, false, false, false, false }},
                {"User",                        new bool[] {false, false, false, false, false }},
                {"Customer",                    new bool[] {false, true, false, false, false }},
                {"Location",                    new bool[] {false, true, false, false, false }},
                {"CleaningTask",                new bool[] {false, true, false, false, false }},
                {"QualityReport",               new bool[] {false, false, false, false, false }},
                {"CompletedTask",               new bool[] {true, true, false, false, false }},
                {"Login",                       new bool[] {false, false, false, false, false }},
                {"Permission",                  new bool[] {false, false, false, false, false }},
                {"RegularTask",                 new bool[] {false, false, false, false, false }},
                {"WindowTask",                  new bool[] {false, false, false, false, false }},
                {"FanCoilTask",                 new bool[] {false, true, false, false, false }},
                {"Chat",                        new bool[] {false, true, true, false, false }}
            };
        
            Dictionary<string, bool[]> LocationManagerPermissions = new Dictionary<string, bool[]>
            {
                //NAME                                      Create Read   Update Delete
                {"Team",                        new bool[] {false, true, false, false, false }},
                {"User",                        new bool[] {false, true, false, false, false }},
                {"Customer",                    new bool[] {false, true, false, false, false }},
                {"Location",                    new bool[] {false, true, false, false, false }},
                {"CleaningTask",                new bool[] {false, true, false, false, false }},
                {"QualityReport",               new bool[] {false, true, false, false, false }},
                {"CompletedTask",               new bool[] {false, true, false, false, false }},
                {"Login",                       new bool[] {false, false, false, false, false }},
                {"Permission",                  new bool[] {false, false, false, false, false }},
                {"RegularTask",                 new bool[] {false, true, false, false, false }},
                {"WindowTask",                  new bool[] {false, true, false, false, false }},
                {"FanCoilTask",                 new bool[] {false, true, false, false, false }},
                {"Chat",                        new bool[] {false, true, true, false, false }}
            };

            Dictionary<string, bool[]> CustomerManagerPermissions = new Dictionary<string, bool[]>
            {
                //NAME                                      Create Read   Update Delete
                {"Team",                        new bool[] {false, true, false, false, false }},
                {"User",                        new bool[] {false, true, false, false, false }},
                {"Customer",                    new bool[] {false, true, false, false, false }},
                {"Location",                    new bool[] {false, true, false, false, false }},
                {"CleaningTask",                new bool[] {false, true, false, false, false }},
                {"QualityReport",               new bool[] {false, true, false, false, false }},
                {"CompletedTask",               new bool[] {false, true, false, false, false }},
                {"Login",                       new bool[] {false, false, false, false, false }},
                {"Permission",                  new bool[] {false, false, false, false, false }},
                {"RegularTask",                 new bool[] {false, true, false, false, false }},
                {"WindowTask",                  new bool[] {false, true, false, false, false }},
                {"FanCoilTask",                 new bool[] {false, true, false, false, false }},
                {"Chat",                        new bool[] {false, true, true, false, false }}
            };

            Dictionary<string, Dictionary<string, bool[]>> RolePermissions = new Dictionary<string, Dictionary<string, bool[]>>{
                {"Master", MasterPermissions},
                {"Human resource", HumanResourcePermissions},
                {"Service leader", ServiceLeaderPermissions},
                {"Sales", SalesPermissions},
                {"Regular cleaning assistant", RegularCleaningAssistantPermissions},
                {"Window cleaning assistant", WindowCleaningAssistantPermissions},
                {"Fan coil cleaning assistant", FanCoilCleaningAssistantPermissions},
                {"Location manager", LocationManagerPermissions},
                {"Customer manager", CustomerManagerPermissions},
            };


            var permissions = new Permission[PermissionNames.Length];

            for (var i = 0; i < permissions.Length; i++) {
                permissions[i] = new Permission { Name = PermissionNames[i] };
            }
            context.Permission.AddRange(permissions);
			context.SaveChanges();

            var roles = new Role[RolePermissions.Keys.Count()];

			for (var i = 0; i < roles.Length; i++)
			{
                roles[i] = new Role { Name = RolePermissions.Keys.ToList()[i] };
			}
            context.Role.AddRange(roles);
			context.SaveChanges();

            List<PermissionsTemplate> permissionTemplates = new List<PermissionsTemplate>();
            foreach(var role in roles) {
                foreach(var permission in permissions) {

                    bool[] CRUDD = RolePermissions[role.Name][permission.Name];

                    permissionTemplates.Add(new PermissionsTemplate {
                        RoleID = role.ID, 
                        PermissionID = permission.ID, 
                        Create = CRUDD[0],
                        Read   = CRUDD[1],
                        Update = CRUDD[2],
                        Delete = CRUDD[3]
                    });
                }
            }

            context.PermissionsTemplate.AddRange(permissionTemplates);
            #endregion



/*
            var loginRepo = new LoginRepository(context);

            var login = new Login { Password = LoginRepository.HashPassword("123"), UserName = "master" };
            var user = new User.DB { Email = "rent-app@outlook.com", FirstName = "Master", LastName = "Kanobi", Login = login, RoleID=1 };

            context.Login.Add(login);
            context.SaveChanges();

            context.User.Add(user);
            context.SaveChanges();

            loginRepo.CreatePermissions(user, 1);
*/



            /*
            var customers = new Customer[]
            {
                new Customer{Name = "H&M", Created = DateTime.Now, MainUserID = 1, Status = Customer.CustomerStatus.Customer}
            };
            context.Customer.AddRange(customers);
            context.SaveChanges();
            */

            /*
            var employeeUsers = new EmployeeUser[]
            {
                new EmployeeUser { Email = "mbel@itu.dk"}
            };
            context.AddRange(employeeUsers);
            context.SaveChanges();

            var customerUsers = new CustomerUser[]
            {
                new CustomerUser {CustomerID = 1}
            };*/

            /*
            var users = new User[]
            {
                new User { LoginID = 1 },
                new User { LoginID = 2 }
            };
            context.AddRange(users);
            context.SaveChanges();
            */

            /*

            var translation = new Translation[]
            {
                new Translation{ },
                new Translation{ },
                new Translation{ },
                new Translation{ },
                new Translation{ },
                new Translation{ },
                new Translation{ },
                new Translation{ },
                new Translation{ },
                new Translation{ },
                new Translation{ },
                new Translation{ },
                new Translation{ },
                new Translation{ },
            };
            context.Translation.AddRange(translation);
            context.SaveChanges();

            var translationEntries = new TranslationEntry[]
            {
                new TranslationEntry{Language = Language.DA, Text = "Stuen", TranslationID = 1},
                new TranslationEntry{Language = Language.EN, Text = "Ground floor", TranslationID = 1},
                new TranslationEntry{Language = Language.DA, Text = "1.", TranslationID = 2},
                new TranslationEntry{Language = Language.DA, Text = "2.", TranslationID = 3},
                new TranslationEntry{Language = Language.DA, Text = "3.", TranslationID = 4},
                new TranslationEntry{Language = Language.DA, Text = "4.", TranslationID = 5},
                new TranslationEntry{Language = Language.DA, Text = "5.", TranslationID = 6},
                new TranslationEntry{Language = Language.DA, Text = "6.", TranslationID = 7},
                new TranslationEntry{Language = Language.DA, Text = "Køkken", TranslationID = 8},
                new TranslationEntry{Language = Language.EN, Text = "Kitchen", TranslationID = 8},
                new TranslationEntry{Language = Language.DA, Text = "Toilet", TranslationID = 9},
                new TranslationEntry{Language = Language.EN, Text = "Toilet", TranslationID = 9},
                new TranslationEntry{Language = Language.DA, Text = "Kontor", TranslationID = 10},
                new TranslationEntry{Language = Language.EN, Text = "Office", TranslationID = 10},
            };
            context.TranslationEntry.AddRange(translationEntries);
            context.SaveChanges();*/

            /*var cleaningPlans = new CleaningPlan[]
            {
                new CleaningPlan{LocationID = 3},
                new CleaningPlan{LocationID = 4}
            };
            foreach (CleaningPlan cleaningPlan in cleaningPlans)
            {
                context.CleaningPlan.Add(cleaningPlan);
            }
            context.SaveChanges();*/


            /*
            var cleaningDescriptions = new CleaningDescription[]
            {
                new CleaningDescription{Description = "Støvsug", PricePerSquareMeter = 55},
                new CleaningDescription{Description = "Vask gulv", PricePerSquareMeter = 95}
            };

            var floors = new Floor[]
            {
                new Floor{TranslationID = 1, Description = "Stuen"},
                new Floor{TranslationID = 2, Description = "1."},
                new Floor{TranslationID = 3, Description = "2."},
                new Floor{TranslationID = 4, Description = "3."},
                new Floor{TranslationID = 5, Description = "4."},
                new Floor{TranslationID = 6, Description = "5."},
                new Floor{TranslationID = 7, Description = "6."},
            };
            foreach (Floor floor in floors)
            {
                context.Floor.Add(floor);
            }
            context.SaveChanges();

            var areas = new Area[]
            {
                new Area{TranslationID = 8, Description = "Køkken"},
                new Area{TranslationID = 9, Description = "Toilet"},
                new Area{TranslationID = 10, Description = "Kontor"},
            };
            foreach (var area in areas)
            {
                context.Area.Add(area);
            }
            context.SaveChanges();
            */



            /*
            var CleaningTask = new CleaningTask[]
            {
                new CleaningTask{AreaID = 1, FloorID = 1, Interval = 1, Quantifier = Quantifier.WEEK, CleaningPlanID = 1, Comment = "I hjørnet", SquareMeters = 25},
                new CleaningTask{AreaID = 1, FloorID = 1, Interval = 1, Quantifier = Quantifier.DAY, CleaningPlanID = 2, Comment = "Random name", SquareMeters = 51},
                new CleaningTask{Frequency="777", AreaID = 2, FloorID = 1, CleaningPlanID = 1, Comment = "Prutskurret", SquareMeters = 61},
                new CleaningTask{Frequency="777", AreaID = 3, FloorID = 1, CleaningPlanID = 1, Comment = "1", SquareMeters = 99},
            };
            context.CleaningTask.AddRange(CleaningTask);
            context.SaveChanges();*/
            /*
            var logins = new Login[]
            {
                new Login{UserName = "Mike", Password = Login.HashAndSaltPassword("1234"), User = users[0]},
                new Login{UserName = "Tobias", Password = Login.HashAndSaltPassword("12345"), User = users[1]},
            };
            foreach (Login login in logins)
            {
                context.Login.Add(login);
            }
            context.SaveChanges();*/

            /*
            var qualityReports = new QualityReport[]
            {
                new QualityReport{CleaningPlan = cleaningPlans[0], Time = DateTime.Now, User = users[0]}
            };
            context.QualityReport.AddRange(qualityReports);
            context.SaveChanges();
            
            var qualityReportItems = new QualityReportItem[]
            {
                new QualityReportItem{CleaningTask = CleaningTask[0], QualityReportID = 1, Comment = "Still ugly", Rating = 2}
            };
            context.QualityReportItem.AddRange(qualityReportItems);
            context.SaveChanges();
            */

        }
    }
}
